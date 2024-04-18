using System;
using System.Collections.Generic;
using System.Linq;
using PV178.Homeworks.HW04.Model;

namespace PV178.Homeworks.HW04
{
    class Program
    {
        static void Main(string[] args)
        {
            var queries = new Queries();

            // - - - - - - - - - - - - - 1. Task - - - - - - - - - - - - - - - -
            
            List<int> result1 = queries
                .FourAttacksWithLightestSharksInVenezuelaQuery();
            //Console.WriteLine("Result 1: " + string.Join(", ", result1));

            var expectedResult1 = new List<int> {2892, 3267, 5081, 22};
            Console.WriteLine($"Result 1 is correct: " +
                $"{result1.SequenceEqual(expectedResult1)}\n");

            // - - - - - - - - - - - - - 2. Task - - - - - - - - - - - - - - - -

            bool result2 = queries.AreAllLongSharksGenderIgnoringQuery();
            //Console.WriteLine($"Result 2: {result2}");
            Console.WriteLine($"Result 2 is correct: {result2 == false}\n");

            // - - - - - - - - - - - - - 3. Task - - - - - - - - - - - - - - - -

            int result3 = queries
                .FortunateSharkAttacksSumWithinMonarchyOrTerritoryQuery();
            //Console.WriteLine($"Result 3: {result3}");
            Console.WriteLine($"Result 3 is correct: {result3 == 1157}\n");

            // - - - - - - - - - - - - - 4. Task - - - - - - - - - - - - - - - -

            Dictionary<string, int> result4 = queries
                .SharksWithoutNicknameAttacksQuery();
            //Console.WriteLine($"Result 4:\n{string.Join("\n", result4)}");

            var expectedResult4 = new Dictionary<string, int>
            {
                {"Nurse shark", 35},
                {"Spinner shark", 34},
                {"Mako shark", 30},
                {"Carpet shark", 33},
                {"Hammerhead shark", 46},
                {"Dusky shark", 31},
                {"Grey reef shark", 37},
                {"Lemon shark", 27},
                {"Blue shark", 18},
                {"Salmon shark", 17},
                {"Blacktip shark", 19}
            };

            Console.WriteLine($"Result 4 is correct: " +
                $"{result4.OrderBy(x => x.Key).SequenceEqual(expectedResult4.OrderBy(x => x.Key))}\n");


            // - - - - - - - - - - - - - 5. Task - - - - - - - - - - - - - - - -

            List<string> result5 = queries
                .WhiteDeathSurvivorsStartingWithKQuery();
            //Console.WriteLine($"Result 5: {string.Join(", ", result5)}");

            var expectedResult5 = new List<string>
            { "K. Tracy", "Karl Kuchnow", "Kazuhiho Kato", "Kevin Thompson" };
            Console.WriteLine($"Result 5 is correct: " +
                              $"{result5.SequenceEqual(expectedResult5)}\n");

            // - - - - - - - - - - - - - 6. Task - - - - - - - - - - - - - - - -

            Dictionary<string, int> result6 = queries
                .FiveSharkSpeciesWithMostFatalitiesQuery();
            //Console.WriteLine($"Result 6:\n{string.Join("\n", result6)}");

            var expectedResult6 = new Dictionary<string, int>
            {
                {"White shark", 283},
                {"Hammerhead shark", 148},
                {"Sevengill shark", 131},
                {"Bronze whaler shark", 129},
                {"Wobbegong shark", 126}
            };
            Console.WriteLine($"Result 6 is correct: " +
                              $"{result6.SequenceEqual(expectedResult6)}\n");

            // - - - - - - - - - - - - - 7. Task - - - - - - - - - - - - - - - -

            string result7 = queries.GetSecretCodeQuery();
            //Console.WriteLine($"Result 7: {result7}");
            Console.WriteLine($"Result 7 is correct: {result7 == "238"}\n");

            // - - - - - - - - - - - - - 8. Task - - - - - - - - - - - - - - - -

            string result8 = queries.GovernmentTypePercentagesQuery()
                                    .Replace('.', ',');
            //Console.WriteLine($"Result 8: {result8}");

            var expectedResult8 = "Republic: 59,9%, " +
                                  "Monarchy: 18,6%, " +
                                  "Territory: 15,8%, " +
                                  "AutonomousRegion: 2,0%, " +
                                  "ParliamentaryDemocracy: 1,6%, " +
                                  "AdministrativeRegion: 0,8%, " +
                                  "OverseasCommunity: 0,8%, " +
                                  "Federation: 0,4%";

            Console.WriteLine($"Result 8 is correct: {result8 == expectedResult8}\n");

            // - - - - - - - - - - - - - 9. Task - - - - - - - - - - - - - - - -

            int result9 = queries.VeryLuckyPeopleCountQuery();
            //Console.WriteLine($"Result 9: {result9}");
            Console.WriteLine($"Result 9 is correct: {result9 == 2}\n");

            // - - - - - - - - - - - - - 10. Task - - - - - - - - - - - - - - - 

            List<Tuple<string, List<SharkSpecies>>> result10 = queries
                .LightestSharksInSouthAmericaQuery();

            Console.WriteLine("Result 10:");

            var sharkIds = result10
               .SelectMany(tuple => tuple.Item2)
               .Select(species => species.Id)
               .ToList();

            var expectedSharkIds = new List<int>
            {
                15, 13, 12, 14, 7, 2, 17, 15, 4, 4, 7, 3, 13, 17, 4, 13, 3, 17, 15
            };

            Console.Write("- SharkIDs: ");
            Console.WriteLine(sharkIds.OrderBy(s => s)
                                      .SequenceEqual(expectedSharkIds
                                                     .OrderBy(s => s)));

            var sizes = result10
                .Select(tuple => tuple.Item2.Count)
                .ToList();

            var expectedSizes = new List<int>
            {
                1, 0, 8, 3, 2, 0, 0, 1, 0, 0, 0, 0, 1, 3
            };

            Console.Write("- Sizes: ");
            Console.WriteLine(sizes.OrderBy(s => s)
                                   .SequenceEqual(expectedSizes
                                                  .OrderBy(s => s)));

            var countries = result10
                .Select(tuple => tuple.Item1).ToList();

            var expectedCountries = new List<string>
            {
                "Argentina",
                "Bolivia",
                "Brazil",
                "Chile",
                "Ecuador",
                "Falkland Islands",
                "French Guiana",
                "Guyana",
                "Colombia",
                "Paraguay",
                "Peru",
                "Suriname",
                "Uruguay",
                "Venezuela"
            };

            Console.Write("- Countries: ");
            Console.WriteLine(countries.OrderBy(c => c)
                                       .SequenceEqual(expectedCountries
                                                      .OrderBy(c => c)));
            Console.WriteLine();
            
            // - - - - - - - - - - - - - 11. Task - - - - - - - - - - - - - - - 

            Dictionary<string, Tuple<int, double>> result11 = queries
                .ContinentInfoAboutSurfersQuery();

            var expectedResult11 = new Dictionary<string, Tuple<int, double>>
            {
                { "Central America", new Tuple<int, double>(4, 20.25) },
                { "Australia", new Tuple<int, double>(23, 24.96) },
                { "Asia", new Tuple<int, double>(2, 58) },
                { "Africa", new Tuple<int, double>(8, 22.33) },
                { "Europe", new Tuple<int, double>(1, 47) },
                { "South America", new Tuple<int, double>(4, 19.67) }
            };

            //Console.WriteLine($"Result 11:\n{string.Join("\n", result11)}");

            Console.Write("Result 11 is correct: ");
            Console.WriteLine(expectedResult11
                .OrderBy(x => x.Key)
                .SequenceEqual(result11.OrderBy(x => x.Key)));
            Console.WriteLine();
            
            // - - - - - - - - - - - - - 12. Task - - - - - - - - - - - - - - - 

            List<string> result12 = queries
                .InfoAboutPeopleThatNamesStartsWithCAndWasInBahamasQuery();

            var expectedResult12 = new List<string>
            {
                "Captain Masson was attacked in Bahamas by Rhincodon typus",
                "C.D. Dollar was attacked in Bahamas by Carcharhinus brachyurus",
                "Carl James Harth was attacked in Bahamas by Carcharhinus brachyurus",
                "Carl Starling was attacked in Bahamas by Carcharhinus amblyrhynchos"
            };

            //Console.WriteLine($"Result12:\n{string.Join("\n", result12)}");
            Console.Write("Result 12 is correct: ");

            Console.WriteLine(result12.OrderBy(x => x)
                .SequenceEqual(expectedResult12.OrderBy(x => x)));

            Console.WriteLine();
            
            // - - - - - - - - - - - - - 13. Task - - - - - - - - - - - - - - - 

            List<string> result13 = queries
                .InfoAboutPeopleThatWasInBahamasHeroicModeQuery();

            var expectedResult13 = new List<string>
            {
                #region PeopleInBahamasAttackedBySharks
                "male was attacked by Isurus oxyrinchus",
                "Patricia Hodge was attacked by Sphyrna lewini",
                "Karl Kuchnow was attacked by Carcharodon carcharias",
                "Doug Perrine was attacked by Carcharhinus brevipinna",
                "Captain Masson was attacked by Rhincodon typus",
                "Kevin G. Schlusemeyer was attacked by Carcharhinus obscurus",
                "14' boat, occupant: Jonathan Leodorn was attacked by Ginglymostoma cirratum",
                "Jerry Greenberg was attacked by Carcharhinus obscurus",
                "Bruce Johnson, rescuer was attacked by Isurus oxyrinchus",
                "Philip Sweeting was attacked by Isurus oxyrinchus",
                "C.D. Dollar was attacked by Carcharhinus brachyurus",
                "Stanton Waterman was attacked by Carcharodon carcharias",
                "Francisco Edmund Blanc, a scientist from National Museum in Paris was attacked by Carcharodon carcharias",
                "Roy Pinder was attacked by Carcharhinus brevipinna",
                "Joanie Regan was attacked by Carcharodon carcharias",
                "Richard  Winer was attacked by Carcharodon carcharias",
                "young girl was attacked by Orectolobus hutchinsi",
                "12' skiff, occupant: E.R.F. Johnson was attacked by Carcharodon carcharias",
                "E.F. MacEwan was attacked by Carcharhinus limbatus",
                "Nick Raich was attacked by Carcharodon carcharias",
                "Krishna Thompson was attacked by Isurus oxyrinchus",
                "Kevin King was attacked by Isurus oxyrinchus",
                "James Douglas Munn was attacked by Sphyrna lewini",
                "John DeBry was attacked by Rhincodon typus",
                "John Petty was attacked by Notorynchus cepedianus",
                "male was attacked by Isurus oxyrinchus",
                "Sean Connelly was attacked by Carcharodon carcharias",
                "Mr. Wichman was attacked by Sphyrna lewini",
                "Tip Stanley was attacked by Carcharias taurus",
                "Roger Yost was attacked by Orectolobus hutchinsi",
                "Luis Hernandez was attacked by Carcharodon carcharias",
                "Max Briggs was attacked by Carcharhinus amblyrhynchos",
                "Markus Groh was attacked by Prionace glauca",
                "male, a sponge Diver was attacked by Carcharhinus brachyurus",
                "Michael Dornellas was attacked by Carcharhinus obscurus",
                "Henry Kreckman was attacked by Notorynchus cepedianus",
                "Katie Hester was attacked by Rhincodon typus",
                "Mark Adams was attacked by Carcharodon carcharias",
                "Leslie Gano was attacked by Orectolobus hutchinsi",
                "Whitefield Rolle was attacked by Sphyrna lewini",
                "Nixon Pierre was attacked by Carcharhinus brachyurus",
                "Sabrina Garcia was attacked by Sphyrna lewini",
                "Benjamin Brown was attacked by Galeocerdo cuvier",
                "Andrew Hindley was attacked by Ginglymostoma cirratum",
                "Bryan Collins was attacked by Galeocerdo cuvier",
                "male was attacked by Rhincodon typus",
                "Kerry Anderson was attacked by Notorynchus cepedianus",
                "Lacy Webb was attacked by Carcharodon carcharias",
                "male was attacked by Carcharias taurus",
                "male was attacked by Carcharhinus obscurus",
                "Russell Easton was attacked by Ginglymostoma cirratum",
                "Wolfgang Leander was attacked by Negaprion brevirostris",
                "Richard Horton was attacked by Ginglymostoma cirratum",
                "Kent Bonde was attacked by Carcharhinus obscurus",
                "Robert Gunn was attacked by Carcharhinus plumbeus",
                "Jim Abernethy was attacked by Ginglymostoma cirratum",
                "Derek Mitchell was attacked by Carcharodon carcharias",
                "Alton Curtis was attacked by Carcharhinus brachyurus",
                "male was attacked by Carcharhinus plumbeus",
                "Burgess & 2 seamen was attacked by Carcharias taurus",
                "Wilber Wood was attacked by Orectolobus hutchinsi",
                "boy was attacked by Carcharhinus brevipinna",
                "Erik Norrie was attacked by Sphyrna lewini",
                "Scott Curatolo-Wagemann was attacked by Ginglymostoma cirratum",
                "Kenny Isham was attacked by Carcharhinus brachyurus",
                "Lowell Nickerson was attacked by Carcharodon carcharias",
                "Peter Albury was attacked by Notorynchus cepedianus",
                "Wyatt Walker was attacked by Ginglymostoma cirratum",
                "William Barnes was attacked by Carcharhinus brachyurus",
                "Valerie Fortunato was attacked by Lamna ditropis",
                "Ken Austin was attacked by Carcharhinus obscurus",
                "John Fenton was attacked by Carcharodon carcharias",
                "Jose Molla was attacked by Carcharodon carcharias",
                "Jane Engle was attacked by Carcharhinus obscurus",
                "Judson Newton was attacked by Carcharhinus limbatus",
                "John Cooper was attacked by Ginglymostoma cirratum",
                "Herbert J. Mann was attacked by Sphyrna lewini",
                "Bruce Cease was attacked by Isurus oxyrinchus",
                "Judy St. Clair was attacked by Ginglymostoma cirratum",
                "Larry Press was attacked by Carcharias taurus",
                "male from pleasure craft Press On Regardless was attacked by Galeocerdo cuvier",
                "Robert Marx was attacked by Notorynchus cepedianus",
                "Renata Foucre was attacked by Carcharhinus obscurus",
                "Hayward Thomas & Shalton Barr was attacked by Sphyrna lewini",
                "Bill Whitman was attacked by Carcharhinus leucas",
                "Eric Gijsendorfer was attacked by Carcharhinus obscurus",
                "Carl James Harth was attacked by Carcharhinus brachyurus",
                "Carl Starling was attacked by Carcharhinus amblyrhynchos",
                "George Vanderbilt was attacked by Carcharias taurus",
                "Kevin Paffrath was attacked by Carcharhinus brachyurus",
                "Erich Ritter was attacked by Rhincodon typus",
                "unknown was attacked by Carcharhinus brevipinna",
                "a pilot was attacked by Carcharhinus leucas",
                "Michael Beach was attacked by Notorynchus cepedianus",
                "Omar Karim Huneidi was attacked by Carcharhinus amblyrhynchos",
                "Richard Pinder was attacked by Carcharhinus brachyurus"
                #endregion
            };

            //Console.WriteLine($"Result13:\n{string.Join("\n", result13)}");
            Console.Write("Result 13 is correct: ");

            Console.WriteLine(result13.OrderBy(x => x)
                .SequenceEqual(expectedResult13.OrderBy(x => x)));

            Console.WriteLine();
            
            // - - - - - - - - - - - - - 14. Task - - - - - - - - - - - - - - - 

            List<string> result14 = queries.InfoAboutFinesInEuropeQuery();

            var states = result14
                .Select(state => state.Split(':')[0]);

            var expectedStates = new List<string>
            {
                #region States
                "Italy",
                "Croatia",
                "Greece",
                "France",
                "Ireland",
                "Albania",
                "Andorra",
                "Austria",
                "Belarus",
                "Belgium",
                "Bosnia and Herzegovina",
                "Bulgaria",
                "Czech Republic",
                "Denmark",
                "Estonia",
                "Faroe Islands",
                "Finland",
                "Germany",
                "Gibraltar",
                "Guernsey",
                "Holy See (Vatican City)",
                "Hungary",
                "Iceland",
                "Isle of Man",
                "Jersey",
                "Kosovo",
                "Latvia",
                "Liechtenstein",
                "Lithuania",
                "Luxembourg"
                #endregion
            };

            //Console.WriteLine($"Result14:\n{string.Join("\n", result14)}");
            Console.WriteLine("Result 14:");
            Console.Write("- States: ");
            Console.WriteLine(expectedStates.OrderBy(s => s)
                .SequenceEqual(states.OrderBy(s => s)));

            var fines = result14
                .Select(state => state.Split(':')[1])
                .Take(6);

            var exptectedFines = new List<string>
            {
                " 17900 EUR",
                " 8900 HRK",
                " 6750 EUR",
                " 3150 EUR",
                " 300 EUR",
                " 0 ALL"
            };

            Console.Write("- Fines: ");
            Console.WriteLine(exptectedFines.SequenceEqual(fines));

            // - - - - - - - - - - - - - Close window - - - - - - - - - - - - - 
            Console.Write("\nPress any key to close this window: ");
            Console.ReadKey();
        }
    }
}
