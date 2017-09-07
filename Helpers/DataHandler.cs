using Android.App;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlarmClock
    
   
{ 
    public static class DataHandler
    {
        public static Save LoadData()
        {
            if (File.Exists(Path.Combine(Application.Context.FilesDir.Path, "save.json")))
            {
                Save save = null;

                using (StreamReader file = File.OpenText(Path.Combine(Application.Context.FilesDir.Path, "save.json")))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    save = (Save)serializer.Deserialize(file, typeof(Save));
                }

                return save;
            }
            else
                return null;
        }

        public static void SaveData(List<Alarm> alarms)
        {
            Save save = new Save();
            save.Alarms = alarms;
            string json = JsonConvert.SerializeObject(save, Formatting.Indented);
            JObject jSave = JObject.Parse(json);
            using (StreamWriter file = new StreamWriter(Path.Combine(Application.Context.FilesDir.Path, "save.json")))
            {
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    jSave.WriteTo(writer);
                }
            }
        }
    }
}
