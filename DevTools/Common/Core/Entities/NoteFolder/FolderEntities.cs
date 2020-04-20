using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder
{
    public class FolderShortEntity
    {
        public int Id { get; set; }
        public string Nodes { get; set; }

        public IEnumerable<int> NodeList
        {
            get => !string.IsNullOrWhiteSpace(Nodes) ? JsonConvert.DeserializeObject<IEnumerable<int>>(Nodes) : new List<int>();
            set => Nodes = value != null && value.Any() ? JsonConvert.SerializeObject(value) : null;
        }
    }

    public class FolderEntity : FolderShortEntity
    {
        public string Name { get; set; }
    }

    public class FolderNode
    {
        public FolderEntity Entity { get; set; }
        public IEnumerable<FolderNode> Nodes { get; set; }
    }

    public class FolderShortNode
    {
        public int Id { get; set; }
        public IEnumerable<FolderShortNode> Nodes { get; set; }
    }
}