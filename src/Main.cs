using System.Linq;
using System.Collections;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Minimalism
{
    [BepInPlugin("goi.ext.minimalism", "Minimalism", "0.1.0")]
    public class Minimalism : BaseUnityPlugin
    {
        private ConfigEntry<bool> modEnabled;
        private ConfigEntry<Color> playerColor;
        private ConfigEntry<Color> foregroundColor;
        private ConfigEntry<Color> backgroundColor;

        private Material blank = null;
        
        private void Awake()
        {
            modEnabled = Config.Bind(
                "", "Enabled", true,
                "Whether or not the mod should be enabled"
            );
            playerColor = Config.Bind(
                "Colors", "Player Color", Color.white,
                "What color you want the player to be"
            );
            foregroundColor = Config.Bind(
                "Colors", "Foreground Color", Color.black,
                "What color you want the player to be"
            );
            backgroundColor = Config.Bind(
                "Colors", "Background Color", Color.grey,
                "What color you want the player to be"
            );
            SceneManager.sceneLoaded += OnSceneLoad;
        
            Logger.LogInfo("GOI Minimalism is ready");
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            // Main menu
            if (scene.name == "Loader") {
                // Always initialize the material every main menu load regardless of if the mod is enabled or not, that way
                // you can enable or disable it in the midst of gameplay
                GameObject target = GameObject.Find("Canvas/Column/Settings");
                var graphic = target.GetComponent<Graphic>();
                if (graphic != null) {
                    Material mat = graphic.materialForRendering;

                    if (mat != null) {
                        blank = new Material(mat);
                        Logger.LogInfo("Got the material, got his ass.");
                    }
                }
            }
            // Don't apply the rest if the mod's disabled
            if (!modEnabled.Value)
                return;

            if (scene.name == "Mian") {
                // Setup the background first, simplest to do
                Camera BGCamera = GameObject.Find("Main Camera/BGCamera").GetComponent<Camera>();
                BGCamera.farClipPlane = 1.0f;
                BGCamera.backgroundColor = backgroundColor.Value;
                Renderer[] noCollide = GameObject.Find("Mountain_NoCollide").GetComponentsInChildren<Renderer>();
                Renderer[] mountain = GameObject.Find("Mountain").GetComponentsInChildren<Renderer>();
                Renderer[] bucket = GameObject.Find("Rope4").GetComponentsInChildren<Renderer>();
                Renderer[] props = GameObject.Find("Props").GetComponentsInChildren<Renderer>();
                Renderer[] snake = GameObject.Find("Snake").GetComponentsInChildren<Renderer>();

                Renderer[] renderers = mountain.Concat(noCollide).Concat(props).Concat(bucket).Concat(snake).ToArray();

                foreach (var rend in renderers) {
                    rend.material = blank;
                    rend.material.color = foregroundColor.Value;

                    if (rend.materials.Length > 1) {
                        Material[] mats = new Material[rend.materials.Length];
                        for (int i = 0; i < mats.Length; i++) {
                            mats[i] = new Material(blank);
                            mats[i].color = foregroundColor.Value;
                        }
                        rend.materials = mats;
                    }
                }

                // Blank out the player
                Renderer[] player = GameObject.Find("Player").GetComponentsInChildren<Renderer>();
                foreach (var rend in player) {
                    if (rend.gameObject.name != "Shadow") {
                        rend.material = blank;
                        rend.material.color = playerColor.Value;
                    }
                }

                // Make a list of names of terrible terrible cosmetic objects that must be destroyed for the game to be visible
                string[] terribleTerribleObjects = new string[] {
                    "Props/Trees/Broadleaf_Desktop",
                    "Mountain_NoCollide/wet_grass",
                    "Mountain_NoCollide/wet_grass (1)",
                    "Mountain_NoCollide/Rock16_1_A",
                    "Mountain_NoCollide/Rock16_1_A (1)",
                    "Mountain_NoCollide/Rock16_1_A (2)",
                    "Mountain_NoCollide/Rock16_1_A (3)",
                    "Mountain_NoCollide/Rock16_1_A (4)",
                    "Mountain_NoCollide/Rock16_1_A (5)",
                    "Mountain_NoCollide/Rock16_1_A (6)",
                    "Mountain_NoCollide/Rock16_4_A",
                    "Mountain_NoCollide/Glow",
                    "Mountain_NoCollide/Glow (1)",
                    "Mountain_NoCollide/Snake Sign",
                    "Mountain_NoCollide/FrenchDoor",
                    "Mountain_NoCollide/Brick_Wall",
                    "Mountain_NoCollide/elektrobox",
                    "Mountain_NoCollide/plant_Mandevilla_Vine_01",
                    "Mountain_NoCollide/plant_Mandevilla_Vine_02"
                };

                foreach (string name in terribleTerribleObjects) {
                    Destroy(GameObject.Find(name));
                }
            }
        }
    }
}

/* The code used to black things out ingame in unity explorer
Material uiMat = GameObject.Find("Canvas/InGame Menu/Panel").GetComponent<CanvasRenderer>().GetMaterial();
Material blank = new Material(uiMat);

Renderer[] renderers = GameObject.Find("Props").GetComponentsInChildren<Renderer>();

foreach (var rend in renderers) {
	rend.material = blank;
	rend.material.color = new Color32(0x11, 0x11, 0x1b, 0xff);
	if (rend.materials.Length > 1) {
		Material[] mats = new Material[rend.materials.Length];
		for (int i = 0; i < mats.Length; i++) {
			mats[i] = new Material(blank);
			mats[i].color = new Color32(0x11, 0x11, 0x1b, 0xff);
		}
		rend.materials = mats;
	}
}
*/
