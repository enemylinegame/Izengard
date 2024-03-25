using Tools.DragAndDrop;
using UnityEngine;

namespace BrewSystem.Model
{
    public class IngridientDragDataModel : IDraggedData
    {
        public int Id { get; set; }

        public Sprite Sprite { get; set; }
    }
}
