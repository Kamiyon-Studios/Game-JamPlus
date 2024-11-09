Hello Mr.! #speaker:Pink #layout:right
Hello Ms.! #speaker:player #layout:left
-> main

=== main ===
Do you prefer mornings, afternoons, or evenings? #speaker:Pink #layout:right
    + [Mornings]
        -> chosen("Mornings")
    + [Afternoons]
        -> chosen("Afternoons")
    + [Evenings]
        -> chosen("Evenings")
     
=== chosen(answer) ===
{answer == "Mornings":
    Early bird, huh? Mornings are great for a fresh start!
    }
{answer == "Afternoons":
    The afternoon vibe is perfect for a productive day.
    }
{answer == "Evenings":
    Night owl! Evenings have their own peaceful charm.
    }

-> END