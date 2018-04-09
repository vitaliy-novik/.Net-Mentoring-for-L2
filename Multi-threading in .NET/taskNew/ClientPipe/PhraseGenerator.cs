using System;
using System.Collections.Generic;

namespace ClientPipe
{
	class PhraseGenerator
	{
		Random random;

		public PhraseGenerator()
		{
			random = new Random();
		}

		public string GetPhrase()
		{
			return randomPhrases[random.Next(randomPhrases.Count - 1)];
		}

		static List<string> randomPhrases = new List<string>
		{
			"Like Father Like Son",
			"Drawing a Blank",
			"Dropping Like Flies",
			"Roll With the Punches",
			"Hear, Hear",
			"Tough It Out",
			"Right Off the Bat",
			"High And Dry",
			"Read 'Em and Weep",
			"Keep On Truckin'",
			"Not the Sharpest Tool in the Shed",
			"Cry Over Spilt Milk",
			"Wouldn't Harm a Fly",
			"Hit Below The Belt",
			"Right Out of the Gate",
			"Jig Is Up",
			"Birds of a Feather Flock Together",
			"Money Doesn't Grow On Trees",
			"Quick On the Draw",
			"Long In The Tooth"
		};
	}
}
