# vgm2mid
Visual Basic Version Of VGM2MID Tool (Original By W.J.Holt)

-- Version
+ v0.1
    - Add YM2151, YM2203, YM2608 support (Note: OPNx SSG and dual chip support may not work properly)
    - Add YM2610 ADPCM-A & ADPCM-B support
    
+ v0.11
    - GZip implemented (.vgz are now supported)
    - Fix path loading errori
    - Fix problems about YM2203 operators
    
+ v0.12
    - Add YM2612 support


-- Issues

There are some known issues in the current project:
- FM instruments are not 100% correctly dumped.
- MIDI note sometimes seems to be glitchy.


(Note: Some older VGM modules which contain less bytes of vgm header are considered to be deprecated and may lead to an error due to version mismatching. 1.51 or newer are recommended)
