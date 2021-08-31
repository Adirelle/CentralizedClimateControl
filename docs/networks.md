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
