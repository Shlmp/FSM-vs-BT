# FSM-vs-BT

There is a base for each AI, where both AIs do the same thing. 

## Objective AI Chosen
IA Overwhelmed: (Si hay pocas "unidades" o personajes en una aea, se acerca a ellos pero si son demasiados se aleja y se va a otro lugar seguro.

## Description
The AI will patrol between 3 different locations. It will continue to patrol these 3 points until it detects 3 or more cubes with the layer "unitMask". Once it detects them, if there are at least 3 of those but less than 5, the AI will try to got into the center of the group. However, if there are more than 5 of those, then the AI will run away from the group, once it gets far away, it will try to go back to the group.

## AI Thought Process
The AI has a list of Transforms and it cycles through them, once it gets close enough to its current patrolPoint, it changes to the next one. The AI has a Physics.OverlapSphere, where it detects the amount of "unitMask" objects inside this sphere. Here, if it detects 3 or more, without being 5 or more, it will enter the "ApproachGroup" state, where it will try to go into the center of the group (aka will get the position of every unit and try to calculate where the middle point is). If at any point the AI detects 5 or more units, then the AI will enter the "Flee" state and run away from the group.
Once the AI ran away a certain distance, it will try to go back to the group.

## Difficulties in Development
### FSM


### BT
