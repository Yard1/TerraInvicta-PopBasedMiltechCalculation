# Unification Pop Based Miltech Calculation

*For Terra Invicta*

This mod changes the formula used to calculate miltech level after two countries have unified. The intention is to make the drop in miltech less dramatic and based on population and number of armies instead of number of regions.

As an example, with this change, US unifying with Canada will not drop the miltech much as opposed to vanilla.

The previous formula:

```
miltech = (joiningNationMiltech * numJoiningNationRegions + unifierMiltech * (numUnifierRegions - numJoiningNationRegions)) / numUnifierRegions;
```

The new formula:

```
joiningNationMultiplier = joiningNationPopulation + ((numJoiningNationArmies + numJoiningNationNavies) * 0.5 * joiningNationPopulation)
unifierMultiplier = unifierPopulation + ((numunifierArmies + numunifierNavies) * 0.5 * unifierPopulation)
miltech = (joiningNationMiltech * joiningNationMultiplier + unifierMiltech * unifierMultiplier) / (joiningNationMultiplier + unifierMultiplier)
```

The mod requires Unity Mod Manager to function. To install Unity Mod Manager, download version 0.25.0 or later version from Nexus Mods (https://www.nexusmods.com/site/mods/21).

Install Unity Mod Manager using Doorstop Proxy installation method.

Mod webpage: https://github.com/Yard1/TerraInvicta-PopBasedMiltechCalculation
