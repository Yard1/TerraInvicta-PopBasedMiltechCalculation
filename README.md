# Unification Pop Based Miltech Calculation

*For Terra Invicta*

This mod changes the formula used to calculate miltech level after two countries have unified. The intention is to make the drop in miltech less dramatic and based on population and number of armies instead of number of regions.

You can choose to either represent armies and navies as flat amounts of population or multipliers to existing population. By default, multipliers of 0.5 are used for both armies and navies (meaning each army is equal to 50% of the nation's population). The settings can be changed using Unity Mod Manager Settings functionality.

As an example, with this change, US unifying with Canada will not drop the miltech much as opposed to vanilla.

The previous formula:

```
miltech = (joiningNationMiltech * numJoiningNationRegions + unifierMiltech * (numUnifierRegions - numJoiningNationRegions)) / numUnifierRegions;
```

The new formulas:

*Non-flat*
```
joiningNationMultiplier = joiningNationPopulation + (numJoiningNationArmies * armyMultiplier * joiningNationPopulation) + (numJoiningNationNavies * navyMultiplier * joiningNationPopulation)
unifierMultiplier = unifierPopulation + (numUnifierArmies * armyMultiplier * unifierPopulation) + (numUnifierNavies * navyMultiplier * unifierPopulation)
miltech = (joiningNationMiltech * joiningNationMultiplier + unifierMiltech * unifierMultiplier) / (joiningNationMultiplier + unifierMultiplier)
```

*Flat*
```
joiningNationMultiplier = joiningNationPopulation + (numJoiningNationArmies * armyPopulationValue) + (numJoiningNationNavies * navyPopulationValue)
unifierMultiplier = unifierPopulation + (numUnifierArmies * armyPopulationValue) + (numUnifierNavies * navyPopulationValue)
miltech = (joiningNationMiltech * joiningNationMultiplier + unifierMiltech * unifierMultiplier) / (joiningNationMultiplier + unifierMultiplier)
```

The mod requires Unity Mod Manager to function. To install Unity Mod Manager, download version 0.25.0 or later version from Nexus Mods (https://www.nexusmods.com/site/mods/21).

Install Unity Mod Manager using Doorstop Proxy installation method.

Mod webpage: https://github.com/Yard1/TerraInvicta-PopBasedMiltechCalculation
