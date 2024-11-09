Hello there! #speaker:Green #layout:right
I have a question for you.
What is it? #speaker:player #layout:left
-> main

=== main ===
Are you Happy? #speaker:Green #layout:right
    + [Yes]
        -> chosen(0)
    + [No]
        -> chosen(1)
    + [Maybe?]
        -> chosen(2)
     
=== chosen(answer) ===
{answer == 0: That's great! Keep smiling!.}
{answer == 1: I'm sorry to hear that. Hope things get better soon.}
{answer == 2: Uncertainty is okay. Take your time to figure things out.}

Thank You! #speaker:player #layout:left
-> END