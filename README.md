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
Since I am using Scriptable Objects, and since SO are stored as assets and assets cannot reference objects in a live scene, only the other way around, I cannot just give the SO a list of the patrolPoints, instead, I had to give the AI the list of patrolPoints.

### BT
Ever since the beginning, the AI did not wanted to change between states, and it will only patrol regardless of how many units were around. Not only that, but, if the AI detected multiple units (either 3 or 5, did not matter which one) the navMesh broke and the AI no longer cycle through the patrolPoints, instead just staying in its present point. Not only that, but the code only tried to detect the units when starting the scene, which complicated things even further, as it only tried to change between leaves at the start, and if it didn't detect enough, it went to patrol (as intended), however, if at any point it detected any amount of units necessary to change leaf (3 or 5), it didn't change and continue patroling, but this time not cycling through its patrolPoints. Both of these were later fixed, one by forcing the patrolPoint to cycle, and the other one by changing some stuff inside the `Strategies.cs` and forcing the AI to check every frame. 

## Which One was better?
**FSM.**

This is because, in this case, changing between the 3 states (Patrol  --  Approach  --  Flee) is much easier to read and modify. This is because it only needed transitions between each state, while the BT needed to be reset carefully otherwise it will not work as intended, and it is also much more difficult to understand.

## Diagrams
### FSM
<img width="510" height="709" alt="FSM drawio" src="https://github.com/user-attachments/assets/818e3af2-9109-4877-83d3-683eeb71ccc2" />

### BT
<img width="501" height="721" alt="BT drawio" src="https://github.com/user-attachments/assets/aee990f1-2d17-4b48-bdfb-6c3889763021" />
