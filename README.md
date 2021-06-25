# GSHock
For my capstone project at Kansas State University, I developed a simulation of a G-Shock watch in C# using a state table to implement a state diagram. The "watch" uses a Windows Form for its GUI, and implements full functionality of a G-Shock (time, time set, alarm, alarm set, stopwatch).

The idea behind the state table is to have a table of a pre-defined struct for each entry (each entry has two values: the next state to go to and a delegate to the action to take in that state). Each row of the 2-D array corresponds to a state, and each column corresponds to an input that can change the system. For this system, states were set to match the state diagram in the repo, and inputs were based on each of the four buttons, also to match behavior found in the state diagram.
