﻿namespace NitrogenMod.Items
{
    using System.Collections.Generic;
    using SMLHelper.V2.Crafting;

    class ReinforcedStillSuit : ReinforcedSuitsCore
    {
        public ReinforcedStillSuit()
            : base(classID: "reinforcedstillsuit", friendlyName: "Reinforced Still Suit", description: "An upgraded still suit capable of protecting the user between 500 and 1300m.")
        {
            OnFinishedPatching += SetStaticTechType;
        }

        protected override TechType BaseType { get; } = TechType.Stillsuit;
        protected override EquipmentType DiveSuit { get; } = EquipmentType.Body;

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(3)
                {
                    new Ingredient(TechType.Stillsuit, 1),
                    new Ingredient(TechType.Diamond, 2),
                    new Ingredient(TechType.AramidFibers, 1),
                }
            };
        }

        private void SetStaticTechType() => ReinforcedSuit3ID = this.TechType;
    }
}
