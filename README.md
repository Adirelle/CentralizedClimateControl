# Centralized Climate Control

Centralized air conditioning system for RimWorld.

Build networks of pulsed air pipes and machines to keep your colonists cool.

This a sequel to [ColdToad's mod](https://steamcommunity.com/sharedfiles/filedetails/?id=973091113).

## Air networks

An air network consists of several dedicated buildings linked together by air pipes.

Air throughput in a given network is limited by both its intake and exhaust capacities.
Ideally you want both to be equal to avoid underusing your intake fans or underventilating
your rooms.

### Air pipes

They carry the air between the different buildings of the network. One pipe actually
host smaller inner pipes to keep different air flows from mixing with each other.
This way you can place your buildings wherever you want.

Air pipes are colored. There are three colors: red, blue and cyan. Air pipes of a given
color do not connect with pipes of other colors. This allows to build up to three
separate networks in tight spaces without fuss.

Covered variants of air pipes also exist, so you can cool your nice looking rooms
without ruining their beauty.

By default, buildings connect to any air pipe but you can restrict them to connect
only to pipes of a given color.

### Intake fans

The intake fans suck ambiant air up and inject it in the air network. They need some
free space around them to work to their full capacity.

### Temperature control units

The temperature control units (TCU) process air coming from the intake fans. They cool
or heat air to reach the target temperature. TCU are optional: a network without any
TCU just moves air from the intake fans to the vents. When cooling they exhaust excess
heat in the adjacent tiles.

They have a limited throughput though, so you may need to add as many TCUs as needed.
If the throughput of all TCUs of a network is lower than this current throughput,
some unconditionned air will be mixed with the output of the TCUs.

The efficiency of a TCU depends on two parameters:

- throughput load: a TCU at full capacity is less efficient than a TCU at half capacity, for example.
- temperature gap: TCU are less efficient when the target temperature is much lower/higher than the temperature of the incoming air.

If the TCUs of a given network fail to reach the target temperature, you can add more of them to split the load amongst them.

### Vents

Vents push conditioned air from the air network into adjacent tiles. They exist in different
shape and size. Bigger rooms needs bigger vents.

## Source

The source is available on [GitHub](https://github.com/Adirelle/CentralizedClimateControl).

## Error reporting

- See if the the error persists if you just have this mod and its requirements active.
- If not, try adding your other mods until it happens again.
- Reports errors to the [GitHub repository](https://github.com/Adirelle/CentralizedClimateControl/issues).
- Post your error-log using [HugsLib](https://steamcommunity.com/workshop/filedetails/?id=818773962) and command Ctrl+F12.
- Do not report errors by making a discussion-thread, I get no notification of that.

## Changelog

See the [dedicated file](https://github.com/Adirelle/CentralizedClimateControl/blob/main/CHANGELOG.md).

## Authors and contributors

- [vasumahesh](https://steamcommunity.com/id/vasumahesh)
- [Mlie](https://steamcommunity.com/id/Mlie)
- [Adirelle](https://github.com/Adirelle)
- [Jdalt40](https://github.com/Jdalt40) [B19 changes]
- [spoden](https://github.com/spoden)
- [BrainInBlack](https://github.com/BrainInBlack)

## Credits

- [carlgraves](https://ludeon.com/forums/index.php?action=profile;u=19514) for "Central Heating"
- [Redist Heat](https://ludeon.com/forums/index.php?topic=21770.0)
- [Dubs Hygiene Mod](https://ludeon.com/forums/index.php?topic=29043.msg341113#msg341113)

## License

MIT License
