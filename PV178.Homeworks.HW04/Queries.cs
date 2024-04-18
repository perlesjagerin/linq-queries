using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PV178.Homeworks.HW04.DataLoading.DataContext;
using PV178.Homeworks.HW04.DataLoading.Factory;
using PV178.Homeworks.HW04.Model;
using PV178.Homeworks.HW04.Model.Enums;
using System.Text.RegularExpressions;

namespace PV178.Homeworks.HW04
{
    public class Queries
    {
        private IDataContext? _dataContext;
        public IDataContext DataContext => _dataContext ??= new DataContextFactory().CreateDataContext();

        /// <summary>
        /// Vyšetrovatelia Venezuelských útokov od nás vyžadujú zvláštnu informáciu.
        /// -----------------------
        /// Vráti zoznam prvých 4 ID útokov, ktoré sa udiali v štáte Venezuela
        /// zoradených vzostupne podľa váhy útočných žralokov.
        /// </summary>
        /// <returns>The query result</returns>
        public List<int> FourAttacksWithLightestSharksInVenezuelaQuery()
        {
            int venezuelaId = DataContext.Countries
                .Where(country => country.Name == "Venezuela" &&
                                  country.CountryCode == "VE")
                .Select(country => country.Id)
                .SingleOrDefault();

            var attacksAndSharks = DataContext.SharkAttacks
                .Join(DataContext.SharkSpecies,
                      attack => attack.SharkSpeciesId,
                      shark => shark.Id,
                      (attack, shark) => new {
                          attack.Id, attack.CountryId, shark.Weight});

            List<int> attackIds = attacksAndSharks
                .Where(result => result.CountryId == venezuelaId)
                .OrderBy(result => result.Weight)
                .Select(result => result.Id)
                .Take(4)
                .ToList();

            return attackIds;
        }

        /// <summary>
        /// Vedci robia štúdiu o tom, či dlhé žraloky pri svojich útokoch berú ohľad
        /// na pohlavie obete. Pomôžte im preto zistiť, či boli zaznamenané útoky
        /// na obidve pohlavia pre každý druh žraloka dlhšieho ako 2 metre.
        /// -----------------------
        /// Vráti informáciu či každý druh žraloka, ktorý je dlhší ako 2 metre
        /// útočil aj na muža aj na ženu.
        /// </summary>
        /// <returns>The query result</returns>
        public bool AreAllLongSharksGenderIgnoringQuery()
        {
            IEnumerable<Sex> genders = DataContext.SharkAttacks
                .Join(DataContext.AttackedPeople,
                    attack => attack.AttackedPersonId,
                    person => person.Id,
                    (attack, person) => new { attack.SharkSpeciesId,
                                              person.Sex })

                .Join(DataContext.SharkSpecies
                    .Where(shark => shark.Length > 2),
                    joined => joined.SharkSpeciesId,
                    shark => shark.Id,
                    (joined, shark) => joined.Sex);

            bool result = genders
                .Where(sex => sex != Sex.Unknown)
                .Distinct()
                .Count() == 2;

            return result;
        }

        /// <summary>
        /// Skupina aktivistov tvrdí, že žraloky v istých druhoch krajín na ľudí neútočia
        /// s úmyslom zabiť, ale sa chcú s nimi iba hrať. Pomôžte zistiť, koľko je takých
        /// prípadov v krajinách s vládnou formou typu 'Monarchy' alebo 'Territory.
        /// -----------------------
        /// Vráti súhrnný počet žraločích utokov, pri ktorých nebolo preukázané že skončili fatálne.
        /// Požadovany súčet vykonajte iba pre krajiny s vládnou formou typu 'Monarchy' alebo 'Territory'.
        /// </summary>
        /// <returns>The query result</returns>
        public int FortunateSharkAttacksSumWithinMonarchyOrTerritoryQuery()
        {
            return DataContext.Countries
                    .Where(country =>
                           country.GovernmentForm == GovernmentForm.Monarchy ||
                           country.GovernmentForm == GovernmentForm.Territory)
                .Join(DataContext.SharkAttacks
                    .Where(attack =>
                           attack.AttackSeverenity != AttackSeverenity.Fatal),
                    country => country.Id,
                    attack => attack.CountryId,
                    (country, attack) => new { country, attack })
                .Count();
        }

        /// <summary>
        /// Žraločí experti chcú vytvoriť prezývky pre žralokov, ktoré ešte prezývku nemajú.
        /// Chcú ich odvodiť od počtu krajín, v ktorých útočili.
        /// -----------------------
        /// Vráti slovník, ktorý pre každého žraloka, ktorý nemá nejakú prezývku (AlsoKnownAs)
        /// uchováva počet krajín, v ktorých útočil. Ako kľúč sa použije meno žraloka.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, int> SharksWithoutNicknameAttacksQuery()
        {
            var attacks = DataContext.SharkAttacks
                .Join(DataContext.Countries,
                    attack => attack.CountryId,
                    country => country.Id,
                    (attack, country) => new { attack.SharkSpeciesId,
                                               attack.CountryId })
                .Distinct();

            Dictionary<string, int> sharks = DataContext.SharkSpecies
                .Where(shark => string.IsNullOrEmpty(shark.AlsoKnownAs))
                .ToDictionary(shark => shark.Name ?? "",
                              shark => attacks
                                .Count(attack => attack.SharkSpeciesId ==
                                                 shark.Id));

            return sharks;
        }

        /// <summary>
        /// Organizácia spojených žraločích národov vyhlásila súťaž, v ktorej každému,
        /// kto spĺňa dané podmienky, vyplatia zaujímavé hodnotné ceny.
        /// -----------------------
        /// Nájdete mená všetkých osôb, ktore od 3.3. 1960 do 12.11. 1980 (vrátane)
        /// zaručene prežili napadnutie žraloka, ktorý sa prezýva "White death".
        /// Z nájdených mien vyberte iba tie, ktoré začínajú písmenom 'K'.
        /// Nájdené mená zoraďte abecedne (a -> z).
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> WhiteDeathSurvivorsStartingWithKQuery()
        {
            int sharkId = DataContext.SharkSpecies
                .Where(shark => shark.AlsoKnownAs == "White death")
                .Select(shark => shark.Id)
                .FirstOrDefault();

            IEnumerable<SharkAttack> attacks = DataContext.SharkAttacks
                .Where(attack =>
                       attack.DateTime >= new DateTime(1960, 3, 3) &&
                       attack.DateTime <= new DateTime(1980, 12, 11) &&
                       attack.AttackSeverenity == AttackSeverenity.NonFatal &&
                       attack.SharkSpeciesId == sharkId);

            IEnumerable<AttackedPerson> people = DataContext.AttackedPeople
                .Where(person => person.Name != null &&
                                 person.Name.StartsWith("K"));

            List<string> result = attacks
                .Join(people,
                    attack => attack.AttackedPersonId,
                    people => people.Id,
                    (attack, people) => people.Name ?? "")
                .OrderBy(name => name)
                .ToList();

            return result;
        }

        /// <summary>
        /// Riaditeľa ZOO by zaujímalo, aké žraloky sú najviac agresívne.
        /// -----------------------
        /// Nájdite 5 žraločích druhov, ktoré majú na svedomí najviac ľudských životov,
        /// druhy zoraďte podľa počte obetí, a výsledok vráťte vo forme slovníku, kde
        ///   kľúč je nazov žraločieho druhu,
        ///   hodnotou je súhrnný počet obetí spôsobený daným druhom žraloka.
        /// </summary>
        /// <returns>The query result</returns>
        public Dictionary<string, int> FiveSharkSpeciesWithMostFatalitiesQuery()
        {
            IEnumerable<string> sharkNames = DataContext.SharkAttacks
                .Where(attack =>
                       attack.AttackSeverenity == AttackSeverenity.Fatal)
                .Join(DataContext.SharkSpecies,
                    attack => attack.SharkSpeciesId,
                    shark => shark.Id,
                    (attack, shark) => shark.Name ?? "");

            Dictionary<string, int> result = sharkNames
                .GroupBy(sharkName => sharkName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .ToDictionary(group => group.Key ?? "",
                              group => group.Count());

            return result;
        }

        /// <summary>
        /// Našiel sa kus papiera a starobylý notebook, ktorý je ale bohužiaľ chránený trojmiestnym
        /// číselným heslom. Aby sme sa do neho dostali, potrebujeme rozlúštiť hádanku na papieri:
        ///
        /// Heslo je súčet písmen, ktoré obsahujú názvy krajín v ktorých
        /// útočil žralok, ktorý má najvyššiu maximálnu rýchlosť zo všetkých žralokov.
        /// -----------------------
        /// Vráti tajné heslo k starobylému notebooku.
        /// </summary>
        /// <returns>The query result</returns>
        public string GetSecretCodeQuery()
        {
            int fastestSharkId = DataContext.SharkSpecies
                .OrderByDescending(shark => shark.TopSpeed)
                .Select(shark => shark.Id)
                .FirstOrDefault();

            IEnumerable<string> countryNames = DataContext.SharkAttacks
                .Where(attack => attack.SharkSpeciesId == fastestSharkId)
                .Join(DataContext.Countries,
                    attack => attack.CountryId,
                    country => country.Id,
                    (attack, country) => country.Name ?? "")
                .Distinct();

            string sumOfCountryLetters = countryNames
                .Sum(countryName => (countryName ?? "").Length)
                .ToString();

            return sumOfCountryLetters;
        }

        /// <summary>
        /// Kolegu by zaujímalo, aké je zastúpenie rôznych druhov foriem vlády vo svete.
        /// -----------------------
        /// Pre všetky hodnoty typu GovernmentForm spočítajte ich percentuálne zastúpenie
        /// (je to jednoducho podiel počtu zemí s daným typom vlády k počtu všetkých zemí).
        /// Z výsledku potom zformujte reťazec (pomocou metódy Aggregate), ktorý byde mať nasledujúci formát:
        /// "{GovermentForm}: {percentualne_zastupenie}%, ...", kde percentuálne zastúpenie bude mať 1 desatinné miesto.
        /// Očakávaný výstup je zoradený zostupne podľa percenta zastúpenia, takže:
        /// "Republic: 59.9%, Monarchy: 18.6%, Territory: 15.8%, ... (na konci výpisu už nie je čiarka)
        /// </summary>
        /// <returns>The query result</returns>
        public string GovernmentTypePercentagesQuery()
        {
            int totalCountries = DataContext.Countries.Count();

            Dictionary<GovernmentForm, string> governmentPercentages =
                DataContext.Countries
                .GroupBy(country => country.GovernmentForm)
                .OrderByDescending(group => group.Count())
                .ToDictionary(group => group.Key,
                              group => string.Format(
                                  "{0:0.0}",
                                  (float)group.Count() / totalCountries * 100));

            string formattedString = governmentPercentages
                .Aggregate("", (result, kvp) =>
                $"{result}{(result != "" ? ", " : "")}{kvp.Key}: {kvp.Value}%");

            return formattedString;
        }

        /// <summary>
        /// Výrobca horoskopov prišiel za vami s otázkou, ktorá mu pomôže vytvoriť horoskop na budúci mesiac.
        /// -----------------------
        /// Vráti koľko sa stalo prípadov, že človek prežil (AttackSeverenity je NonFatal)
        /// po útoku žraloka vo veku, ktorý je vyšší ako priemerná doba života v krajine,
        /// v ktorej bol napadnutý.
        /// </summary>
        /// <returns>The query result</returns>
        public int VeryLuckyPeopleCountQuery()
        {
            IEnumerable<SharkAttack> nonFatalAttacks = DataContext.SharkAttacks
                .Where(attack =>
                       attack.AttackSeverenity == AttackSeverenity.NonFatal);

            var joined = nonFatalAttacks
                .Join(DataContext.AttackedPeople,
                    attack => attack.AttackedPersonId,
                    people => people.Id,
                    (attack, people) => new { attack.CountryId,
                                              people.Age })
                .Join(DataContext.Countries,
                    joined => joined.CountryId,
                    country => country.Id,
                    (joined, country) => new { PersonAge = joined.Age,
                                               country.LifeExpectancy });

            int result = joined
                .Where(joined => joined.PersonAge > joined.LifeExpectancy)
                .Count();

            return result;
        }

        /// <summary>
        /// Zistilo sa, že 10 najľahších žralokov sa správalo veľmi podozrivo počas útokov v štátoch Južnej Ameriky.
        /// ---------------------------------------
        /// Táto funkcia preto vráti zoznam dvojíc, kde pre každý štát z danej množiny
        /// bude uvedený zoznam žralokov z danej množiny, ktorí v tom štáte útočili.
        /// Pokiaľ v nejakom štáte neútočil žiaden z najľahších žralokov, zoznam žralokov bude prázdny.
        /// </summary>
        /// <returns>The query result</returns>
        public List<Tuple<string, List<SharkSpecies>>> LightestSharksInSouthAmericaQuery()
        {
            IEnumerable<Country> southAmericaCountries = DataContext.Countries
                .Where(country => country.Continent == "South America");

            IEnumerable<int> lightestSharkIds = DataContext.SharkSpecies
                .OrderBy(shark => shark.Weight)
                .Select(shark => shark.Id)
                .Take(10);

            List<Tuple<string, List<SharkSpecies>>> result =
                southAmericaCountries
                .Select(country => new Tuple<string, List<SharkSpecies>> (
                    country.Name ?? "",
                    DataContext.SharkAttacks
                        .Where(attack =>
                               attack.CountryId == country.Id &&
                               lightestSharkIds.Contains(attack.SharkSpeciesId))
                        .Join(DataContext.SharkSpecies,
                            attack => attack.SharkSpeciesId,
                            shark => shark.Id,
                            (attack, shark) => shark)
                        .Distinct()
                        .ToList()))
                .ToList();

            return result;
        }

        /// <summary>
        /// Pre všetky kontinenty, v ktorých prišiel o život človek počas surfovania (Activity obsahuje "surf", "SURF" alebo "Surf")
        /// vráti informáciu o počte obetí, ktoré zahynuli pri surfovaní a taktiež ich priemerný vek
        /// zaokrúhlený na dve desatinné miesta.
        /// </summary>
        /// <returns>The query result</returns>
        public  Dictionary<string, Tuple<int, double>> ContinentInfoAboutSurfersQuery()
        {
            IEnumerable<SharkAttack> attacks = DataContext.SharkAttacks
                .Where(attack =>
                       attack.Activity != null &&
                       attack.Activity.ToLower().Contains("surf") &&
                       attack.AttackSeverenity == AttackSeverenity.Fatal);

            var joined = attacks
                .Join(DataContext.Countries,
                    attack => attack.CountryId,
                    country => country.Id,
                    (attack, country) => new
                    {
                        country.Continent,
                        attack.AttackedPersonId
                    })
                .Join(DataContext.AttackedPeople,
                    joined => joined.AttackedPersonId,
                    person => person.Id,
                    (joined, person) => new
                    {
                        joined.Continent,
                        person.Age
                    });

            Dictionary<string, Tuple<int, double>> result = joined
                .Where(joined => joined.Continent != null)
                .GroupBy(joined => joined.Continent)
                .Select(group => new
                {
                    Continent = group.Key,
                    VictimsCount = group.Count(),
                    AverageAge = group.Average(group => group.Age)
                })
                .ToDictionary(
                    item => item.Continent ?? "",
                    item => new Tuple<int, double> (
                        item.VictimsCount,
                        Math.Round((double)(item.AverageAge ?? 0), 2)));

            return result;
        }

        /// <summary>
        /// Vráti zoznam, v ktorom je textová informácia o každom človeku,
        /// ktorého meno začína na písmeno C a na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je v tvare:
        /// {meno človeka} was attacked in Bahamas by {latinský názov žraloka}
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleThatNamesStartsWithCAndWasInBahamasQuery()
        {
            int bahamasId = DataContext.Countries
                .Where(country => country.Name == "Bahamas")
                .Select(country => country.Id)
                .FirstOrDefault();

            IEnumerable<AttackedPerson> peopleStartWithC =
                DataContext.AttackedPeople
                .Where(person => person.Name != null &&
                                 person.Name.StartsWith("C"));

            IEnumerable<SharkAttack> attacksInBahamas =
                DataContext.SharkAttacks
                .Where(attack => attack.CountryId == bahamasId);

            var joined = attacksInBahamas
                .Join(peopleStartWithC,
                    attack => attack.AttackedPersonId,
                    person => person.Id,
                    (attack, person) => new { attack.SharkSpeciesId,
                                              PersonName = person.Name })
                .Join(DataContext.SharkSpecies,
                    joined => joined.SharkSpeciesId,
                    shark => shark.Id,
                    (joined, shark) => new { joined.PersonName,
                                             SharkLatinName = shark.LatinName });

            List<string> result = joined
                .Select(joined => $"{joined.PersonName} was attacked in " +
                                  $"Bahamas by {joined.SharkLatinName}")
                .ToList();

            return result;
        }

        /// <summary>
        /// Vráti zoznam, v ktorom je textová informácia o KAŽDOM človeku na ktorého zaútočil žralok v štáte Bahamas.
        /// Táto informácia je taktiež v tvare:
        /// {meno človeka} was attacked by {latinský názov žraloka}
        ///
        /// POZOR!
        /// Zistite tieto informácie bez spojenia hocijakých dvoch tabuliek (môžete ale použiť metódu Zip)
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutPeopleThatWasInBahamasHeroicModeQuery()
        {
            int bahamasId = DataContext.Countries
                .Where(country => country.Name == "Bahamas")
                .Select(country => country.Id)
                .FirstOrDefault();

            List<int?> peopleIds = DataContext.SharkAttacks
                .Where(attack => attack.CountryId == bahamasId)
                .Select(attack => attack.AttackedPersonId)
                .ToList();

            Dictionary<int, string?> sharks = DataContext.SharkSpecies
                .ToDictionary(shark => shark.Id,
                              shark => shark.LatinName);

            List<AttackedPerson> people = DataContext.AttackedPeople
                .Where(person => peopleIds.Contains(person.Id))
                .ToList();

            List<SharkAttack> attacks = DataContext.SharkAttacks
                .Where(attack => attack.CountryId == bahamasId)
                .ToList();

            List<string> result = people
                .Zip(attacks,
                    (person, attack) => $"{person.Name} was attacked " +
                                        $"by {sharks[attack.SharkSpeciesId]}")
                .ToList();

            return result;
        }


        /// <summary>
        /// Nedávno vyšiel zákon, že každá krajina Európy začínajúca na písmeno A až L (vrátane)
        /// musí zaplatiť pokutu 250 peňazí svojej meny za každý žraločí útok na ich území.
        /// Pokiaľ bol tento útok smrteľný, musia dokonca zaplatiť až 300 peňazí. Ak sa nezachovali
        /// údaje o tom, či bol daný útok smrteľný alebo nie, nemusia platiť nič.
        /// Áno, tento zákon je spravodlivý...
        /// -----------------------
        /// Vráti informácie o výške pokuty každej krajiny Európy začínajúcej na A až L
        /// zoradené zostupne podľa počtu peňazí, ktoré musia tieto krajiny zaplatiť.
        /// Príklad formátu výstupu v prípade, že na Slovensku boli dva smrteľné útoky žralokov,
        /// v Maďarsku 0 útokov a v Česku jeden smrteľný útok a jeden útok, pri ktorom obeť prežila:
        /// Slovakia: 600 EUR
        /// Czech Republic: 550 CZK
        /// Hungary: 0 HUF
        /// </summary>
        /// <returns>The query result</returns>
        public List<string> InfoAboutFinesInEuropeQuery()
        {
            List<Country> countries = DataContext.Countries
                .Where(country => country.Continent == "Europe" &&
                                  Regex.IsMatch(country.Name, "^[A-L].*"))
                                  
                .ToList();

            var fines = countries
                .Select(country => 
                {
                    var attacks = DataContext.SharkAttacks
                        .Where(attack => attack.CountryId == country.Id);

                    var nonFatals = attacks
                        .Where(attack => attack.AttackSeverenity ==
                                         AttackSeverenity.NonFatal)
                        .Count();

                    var fatals = attacks
                        .Where(attack => attack.AttackSeverenity ==
                                         AttackSeverenity.Fatal)
                        .Count();

                    var fine = 0 + nonFatals * 250 + fatals * 300;

                    return new
                    {
                        CountryName = country.Name,
                        Fine = fine,
                        country.CurrencyCode
                    };
                })
                .OrderByDescending(item => item.Fine)
                .ToList();

            List<string> result = fines
                .Select(item => $"{item.CountryName}: " +
                                $"{item.Fine} {item.CurrencyCode}")
                .ToList();

            return result;
        }
    }
}
