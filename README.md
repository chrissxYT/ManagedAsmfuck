# ManagedAsmfuck - C# Compiler, Optimizer and Interpreter for Asmfuck
## Instruction Set Architecture
|instruction|description      |brainfuck operator|binary code|
|-----------|-----------------|------------------|-----------|
|nop        |no operation     |                  |0          |
|inc        |increment at tp  |+                 |1          |
|dec        |decrement at tp  |-                 |2          |
|tsl        |tape shift left  |<                 |3          |
|tsr        |tape shift right |>                 |4          |
|sjp        |set jump position|[                 |5          |
|jpb        |jmp to jp if bool|]                 |6          |
|rac        |read ascii char  |,                 |7          |
|wac        |write ascii char |.                 |8          |
## Registers and Memory
|type   |var|name               |description                                               |
|-------|---|-------------------|----------------------------------------------------------|
|sbyte[]|t  |tape               |the tape that's your memory                               |
|int    |ip |instruction pointer|basically x86's [e/r]ip; points to the current instruction|
|int    |tp |tape pointer       |points to your position on the tape                       |
|int    |jp |jump pointer       |the instruction to jump after on jpb                      |
