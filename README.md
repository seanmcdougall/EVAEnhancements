# EVA Enhancements
EVA Enhancements adds a number of features to the Kerbal Space Program EVA experience.

Copyright (c) 2015 Sean McDougall
Licensed under the MIT License

##Installation
Same as most other mods, copy the EVAEnhancements folder and all its contents to your KSP/GameData folder.

##Current Features

- Kerbal profession and level is now visible in the right-click action menu.  No more wondering if Frolie Kerman is supposed to be an engineer or a scientist.

- Pitch and roll can now be controlled with the keyboard.  No more awkward mouse clicking and dragging... you can now do backflips and cartwheels on the Mun!  By default the controls are mapped as follows:
  
  - Pitch Down - Alpha2
  - Pitch Up - X
  - Roll Left - Z
  - Roll Right - C

  I have found these work well in conjunction with the standard WASDQE controls (while letting you keep your right hand on the mouse for camera control).  If you wish to remap them you can use the "Settings" window, accessible from the EVA right-click action menu.
  
- the EVA "rotate on camera move" setting can now be toggled any time by hitting the SAS toggle (default T).  Enabling it can make some EVA navigation a bit easier, but disabling it gives you more control.

- jetpack power can now be modified any time through a slider in the EVA right-click action menu.  Fuel usage is scaled accordingly.

- the default jetpack power level can be set via the "Settings" window (see above).

- precision controls can now be toggled using the same key as in normal flight mode (default is CapsLock).  This automatically sets your jetpack power to 10% for finer control.

- finally, the Nav Ball is now visible and customized for EVA mode.  
  - the throttle gauge will display your current jetpack power level between 0 and 100%. 
  - the RCS indicator will light to show your if jetpack is enabled.  
  - the SAS indicator will light to show if "rotate on move" is enabled (only if the jetpack is on though)

##Credits

- TriggerAu for the KSPPluginFramework (http://forum.kerbalspaceprogram.com/threads/66503-KSP-Plugin-Framework).  Copyright (c) 2014 David Tregoning, used under the MIT license.
- nlight for the original EVAController module, part of Advanced Fly-By-Wire (https://github.com/AlexanderDzhoganov/ksp-advanced-flybywire).  Copyright (c) 2014 Alexander Dzhoganov, used under the MIT license.
