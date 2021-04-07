//using Rawr.Rogue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Rawr.UnitTests.Rogue
{
    internal class RogueTestCharacter : Character
    {
        public RogueTestCharacter():this(new RogueTalents()){}
        public RogueTestCharacter(RogueTalents talents)
        {
            Class = CharacterClass.Rogue;
            RogueTalents = talents;
            
            
            //OpOv: Direct private access of fields is no longer allowed
            PrivateObject po = new PrivateObject(this,new PrivateType(typeof(RogueTestCharacter)));
            string p = (string)po.GetField("CurrentModel");
            p = "Rogue";
            
            //CurrentModel = "Rogue";
            
            
            //CalculationOptions = new CalculationOptionsRogue();
        }
    }
}