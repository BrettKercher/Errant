using System.IO;
using System.Linq;
using Errant.src.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Entity = GeonBit.UI.Entities.Entity;

namespace Errant.src.Scenes {
    public class MainMenu : Scene {
        
        private Panel mainPanel;
        private Panel worldSelectPanel;
        private Panel newWorldPanel;
        private Panel multiplayerPanel;

        private string newWorldName;
        private WorldSize newWorldSize;

        private string[] worldNames = {};

        public MainMenu(Application _application) : base(_application) {
        }

        public override void Initialize(ContentManager content) {
            base.Initialize(content);

            mainPanel = BuildMainPanel();
            worldSelectPanel = BuildWorldSelectPanel();
            newWorldPanel = BuildNewWorldPanel();
            multiplayerPanel = BuildMultiplayerPanel();
            
            mainPanel.Visible = true;
            worldSelectPanel.Visible = false;
            newWorldPanel.Visible = false;
            multiplayerPanel.Visible = false;
        }

        public override void Dispose(ContentManager content) {
            base.Dispose(content);
            
            UserInterface.Active.RemoveEntity(mainPanel);
            UserInterface.Active.RemoveEntity(worldSelectPanel);
            UserInterface.Active.RemoveEntity(newWorldPanel);

            mainPanel = null;
            worldSelectPanel = null;
            newWorldPanel = null;
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            base.Draw(gameTime, spriteBatch);
        }
        
        private Panel BuildMainPanel() {
            Panel mainMenuPanel = new Panel(new Vector2(400, 350), PanelSkin.Default, Anchor.Center);
            UserInterface.Active.AddEntity(mainMenuPanel);
            
            mainMenuPanel.AddChild(new Header("Errant"));
            mainMenuPanel.AddChild(new HorizontalLine());

            var newGameButton = new Button("Singleplayer", ButtonSkin.Default);
            newGameButton.OnMouseReleased = OnSingleplayerClicked;
            mainMenuPanel.AddChild(newGameButton);
            
            var loadGameButton = new Button("Multiplayer", ButtonSkin.Default);
            loadGameButton.OnMouseReleased = OnMultiplayerClicked;
            mainMenuPanel.AddChild(loadGameButton);
            
            var exitButton = new Button("Quit", ButtonSkin.Default);
            exitButton.OnMouseReleased = OnQuitClicked;
            mainMenuPanel.AddChild(exitButton);
            
            mainMenuPanel.Visible = true;

            return mainMenuPanel;
        }
        
        private Panel BuildWorldSelectPanel() {
            Panel parentPanel = new Panel(new Vector2(400, 425), PanelSkin.Default, Anchor.Center, new Vector2(0, 37.5f));
            UserInterface.Active.AddEntity(parentPanel);
            parentPanel.Opacity = 0;
            parentPanel.Padding = new Vector2(12.5f, 0);
            
            Panel worldMainPanel = new Panel(new Vector2(400, 350), PanelSkin.Default, Anchor.Center, new Vector2(0, -37.5f));
            parentPanel.AddChild(worldMainPanel);
            
            worldMainPanel.AddChild(new Header("Select World"));
            worldMainPanel.AddChild(new HorizontalLine());

            foreach (var worldName in worldNames) {
                
                var worldButton = new Button(worldName, ButtonSkin.Default);
                worldButton.OnMouseReleased = OnWorldSelected;
                worldMainPanel.AddChild(worldButton);
                
            }
            
            worldMainPanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;

            var newButton = new Button("New", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(175, 50));
            newButton.OnMouseReleased = OnWorldSelectNewClicked;
            parentPanel.AddChild(newButton);
            
            var backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomRight, new Vector2(175, 50));
            backButton.OnMouseReleased = OnWorldSelectBackClicked;
            parentPanel.AddChild(backButton);
            
            return parentPanel;
        }

        private Panel BuildNewWorldPanel() {
            Panel parentPanel = new Panel(new Vector2(400, 500), PanelSkin.Default, Anchor.Center, new Vector2(0, 75));
            UserInterface.Active.AddEntity(parentPanel);
            parentPanel.Opacity = 0;
            parentPanel.Padding = new Vector2(12.5f, 0);

            Panel panel = new Panel(new Vector2(400, 425), PanelSkin.Default, Anchor.Center, new Vector2(0, -37.5f));
            parentPanel.AddChild(panel);
            
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Header("Name"));
            panel.AddChild(new HorizontalLine());

            TextInput worldNameBox = new TextInput(false);
            worldNameBox.PlaceholderText = "name";
            worldNameBox.OnValueChange = OnWorldNameValueChanged;
            panel.AddChild(worldNameBox);
            
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Header("Size"));
            panel.AddChild(new HorizontalLine());

            RadioButton smallButton = new RadioButton("Small");
            smallButton.OnValueChange = OnWorldSizeValueChanged;
            
            RadioButton mediumButton = new RadioButton("Medium");
            mediumButton.OnValueChange = OnWorldSizeValueChanged;
            
            RadioButton largeButton = new RadioButton("Large");
            largeButton.OnValueChange = OnWorldSizeValueChanged;
            
            smallButton.Checked = true;

            panel.AddChild(smallButton);
            panel.AddChild(mediumButton);
            panel.AddChild(largeButton);
            
            var newButton = new Button("Create", ButtonSkin.Default, Anchor.BottomLeft, new Vector2(175, 50));
            newButton.OnMouseReleased = OnNewWorldCreateClicked;
            parentPanel.AddChild(newButton);
            
            var backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomRight, new Vector2(175, 50));
            backButton.OnMouseReleased = OnNewWorldBackClicked;
            parentPanel.AddChild(backButton);
            
            return parentPanel;
        }
        
        private Panel BuildMultiplayerPanel() {
            Panel parentPanel = new Panel(new Vector2(400, 425), PanelSkin.Default, Anchor.Center, new Vector2(0, 37.5f));
            UserInterface.Active.AddEntity(parentPanel);
            parentPanel.Opacity = 0;
            parentPanel.Padding = new Vector2(12.5f, 0);
            
            Panel multiplayerMainPanel = new Panel(new Vector2(400, 350), PanelSkin.Default, Anchor.Center, new Vector2(0, -37.5f));
            parentPanel.AddChild(multiplayerMainPanel);
            
            multiplayerMainPanel.AddChild(new Header("Multiplayer"));
            multiplayerMainPanel.AddChild(new HorizontalLine());

            var hostButton = new Button("Host Game", ButtonSkin.Default);
//            hostButton.OnMouseReleased = OnWorldSelected;
            multiplayerMainPanel.AddChild(hostButton);
            
            var joinButton = new Button("Join Game", ButtonSkin.Default);
//            joinButton.OnMouseReleased = OnWorldSelected;
            multiplayerMainPanel.AddChild(joinButton);
            
            var backButton = new Button("Back", ButtonSkin.Default, Anchor.BottomCenter, new Vector2(175, 50));
            backButton.OnMouseReleased = OnMultiplayerBackClicked;
            parentPanel.AddChild(backButton);
            
            return parentPanel;
        }

        private void OnSingleplayerClicked(Entity entity) {
            mainPanel.Visible = false;
            worldSelectPanel.Visible = true;

            RefreshWorldSelectList();
        }
        
        private void OnMultiplayerClicked(Entity entity) {
            mainPanel.Visible = false;
            multiplayerPanel.Visible = true;
        }
        
        private void OnQuitClicked(Entity entity) {
            application.Quit();
        }
        
        private void OnMultiplayerBackClicked(Entity entity) {
            mainPanel.Visible = true;
            multiplayerPanel.Visible = false;
        }

        private void OnWorldSelected(Entity entity) {
            Button selectedWorld = (Button) entity;
            if (selectedWorld == null) {
                return;
            }
            
            GenerationSettings generationSettings = new GenerationSettings();
            generationSettings.name = selectedWorld.ButtonParagraph.Text;
            
            application.SwitchScene(new GenerationScreen(application, generationSettings, true));
        }
        
        private void OnWorldSelectBackClicked(Entity entity) {
            mainPanel.Visible = true;
            worldSelectPanel.Visible = false;
            newWorldPanel.Visible = false;
        }
        
        private void OnWorldSelectNewClicked(Entity entity) {
            mainPanel.Visible = false;
            worldSelectPanel.Visible = false;
            newWorldPanel.Visible = true;
        }

        private void OnWorldNameValueChanged(Entity entity) {
            TextInput text = (TextInput)entity;
            newWorldName = text?.Value;
        }
        
        private void OnWorldSizeValueChanged(Entity entity) {
            RadioButton selectedOption = (RadioButton) entity;

            string value = selectedOption.TextParagraph.Text.ToLower();

            switch (value) {
                case "small":
                    newWorldSize = WorldSize.TINY;
                    break;
                case "medium":
                    newWorldSize = WorldSize.MEDIUM;
                    break;
                case "large":
                    newWorldSize = WorldSize.LARGE;
                    break;
                default:
                    newWorldSize = WorldSize.SMALL;
                    break;
            }
        }
        
        private void OnNewWorldBackClicked(Entity entity) {
            mainPanel.Visible = false;
            worldSelectPanel.Visible = true;
            newWorldPanel.Visible = false;
        }
        
        private void OnNewWorldCreateClicked(Entity entity) {
            
            GenerationSettings generationSettings = new GenerationSettings();
            generationSettings.name = newWorldName;
            generationSettings.seed = 0;
            generationSettings.size = newWorldSize;

            application.SwitchScene(new GenerationScreen(application, generationSettings, false));
        }

        private void RefreshWorldSelectList() {
            
            //refresh world names list
            worldNames = Directory.GetFiles(Config.WorldSaveDirectory, "*.world", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension).ToArray();
            

            if (worldSelectPanel != null) {
                UserInterface.Active.RemoveEntity(worldSelectPanel);
                worldSelectPanel = null;
            }
            
            worldSelectPanel = BuildWorldSelectPanel();
        }
    }
}
