
namespace Rawr.Mistweaver
{
    public class ComparisonCalculationMistweaver : ComparisonCalculationBase
    {
        public override string Name { get; set;}

        private string _desc = string.Empty;
        public override string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public override float OverallPoints { get; set; }

        private float[] subPoints = new float[] { 0f };
        public override float[] SubPoints
        {
            get { return subPoints; }
            set { subPoints = value; }
        }

        private Item _item = null;
        public override Item Item 
        {
            get { return _item; }
            set { _item = value; }
        }

        private ItemInstance _itemInstance = null;
        public override ItemInstance ItemInstance
        {
            get { return _itemInstance; }
            set { _itemInstance = value; }
        }
        
        public override bool Equipped { get; set; }
        public override bool PartEquipped { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: ({1:0.0}O )", Name, OverallPoints);
        }
    }
}
