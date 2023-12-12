using System.Collections.Generic;
using System.Linq;
using ObjectsData;

namespace Main.Journal
{
    public static class JournalManager
    {
        private static Dictionary<TrashCanData, List<TrashData>> _journal;

        public static Dictionary<TrashCanData, List<TrashData>> GetJournal()
        {
            if (_journal is not null && _journal.Count > 0) 
                return _journal;
            
            _journal = new Dictionary<TrashCanData, List<TrashData>>();
            foreach (var trashCanData in ResourceLoader.TrashCanDatas)
            {
                _journal.TryAdd(trashCanData,
                    ResourceLoader.TrashDatas.Where(d => d.Type == trashCanData.Type).ToList());
            }
            return _journal;
        }
    }
}