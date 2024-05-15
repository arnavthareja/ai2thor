using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEditor;
using Newtonsoft.Json.Linq;
using Thor.Procedural.Data;
using Thor.Procedural;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.FirstPerson {
    public class DebugInputField : MonoBehaviour {

        private bool hasCalledInit = false;

        // initialize the procedural generation once on startup
        public void init() {
            Dictionary<string, object> action = new Dictionary<string, object>();
            if (splitcommand.Length == 2) {
                action["gridSize"] = float.Parse(splitcommand[1]);
            } else if (splitcommand.Length == 3) {
                action["gridSize"] = float.Parse(splitcommand[1]);
                action["agentCount"] = int.Parse(splitcommand[2]);
            } else if (splitcommand.Length == 4) {
                action["gridSize"] = float.Parse(splitcommand[1]);
                action["agentCount"] = int.Parse(splitcommand[2]);
                action["makeAgentsVisible"] = int.Parse(splitcommand[3]) == 1;
            }

            action["fieldOfView"] = 90f;
            action["snapToGrid"] = true;
            action["renderInstanceSegmentation"] = true;
            action["renderSemanticSegmentation"] = true;
            action["action"] = "Initialize";
            ActionDispatcher.Dispatch(AManager, new DynamicServerAction(action));
        }

        // Load the specified room into the scene. filename must be a valid JSON file
        public void loadFile(String filename) {
            Dictionary<string, object> action = new Dictionary<string, object>();

            action["action"] = "CreateHouse";
            var ROOM_BASE_PATH = "/Resources/rooms/";

            path = Application.dataPath + ROOM_BASE_PATH + filename;

            var jsonStr = System.IO.File.ReadAllText(path);
            Debug.Log($"jjson: {jsonStr}");

            JObject obj = JObject.Parse(jsonStr);

            action["house"] = obj;
            CurrentActiveController().ProcessControlCommand(new DynamicServerAction(action));
        }

        // Load the room specified by the given JSON into the scene
        public void load(String json) {
            Dictionary<string, object> action = new Dictionary<string, object>();

            action["action"] = "CreateHouse";
            Debug.Log($"jjson: {json}");

            JObject obj = JObject.Parse(json);

            action["house"] = obj;
            CurrentActiveController().ProcessControlCommand(new DynamicServerAction(action));
        }

        void Awake() {
            if (!hasCalledInit) {
                init();
                hasCalledInit = true;
            }

            // example -- load in the basic living room scene
            load('a_living_room.json');

            // TODO integrate with text input area and speech-to-text:
            //   - send input to server to generate a room with Holodeck
            //   - save returned json into a file and call loadFile() on it, or pass in returned json directly into load()
        }
    }
}