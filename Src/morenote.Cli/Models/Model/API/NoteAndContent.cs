using MoreNote.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace morenote_sync_cli.Models.Model.API
{
    public class NoteAndContent
    {
        public Note note { get; set; }
        public NoteContent noteContent { get; set; }

        public static NoteAndContent InstanceFormJson(string json)
        {
            NoteAndContent noteAndContent = JsonSerializer.Deserialize<NoteAndContent>(json, MyJsonConvert.GetOptions());
            return noteAndContent;
        }
    }
}