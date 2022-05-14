using System;
using System.Collections.Generic;
using System.Text;

namespace APIProject.Service.Models.Notification
{
    public class OneSignalModel
    {
    }
    public class OneSignalInput
    {
        public string app_id { get; set; }
        public object data { get; set; }
        public object headings { get; set; }
        public object contents { get; set; }
        public string android_channel_id { get; set; }
        public List<string> include_player_ids { get; set; } = new List<string>();
        public List<string> included_segments { get; set; } = new List<string>();
    }
    public class TextInput
    {
        public string en { get; set; }
    }
    public class NotifyDataModel
    {
        public int id { get; set; }
        public int type { get; set; }
    }
}
