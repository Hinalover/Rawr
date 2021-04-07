using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rawr
{
    // data from http://code.google.com/p/simulationcraft/source/browse/branches/cataclysm/engine/sc_item_data.inc
    public static class RandomSuffix
    {
        private class RandomSuffixDataType
        {
            public int Id;
            public string Suffix;
            public int[] Stat;
            public int[] Multiplier;
        }

        private static RandomSuffixDataType[] FlattenArray(RandomSuffixDataType[] data)
        {
            int maxId = data[data.Length - 1].Id;
            RandomSuffixDataType[] ret = new RandomSuffixDataType[maxId + 1];
            foreach (var d in data)
            {
                ret[d.Id] = d;
            }
            return ret;
        }

        private static readonly RandomSuffixDataType[] RandomSuffixData = FlattenArray(new RandomSuffixDataType[]{
new RandomSuffixDataType() { Id = 5, Suffix = "of the Monkey",          Stat = new int[] {  2802,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 6, Suffix = "of the Eagle",           Stat = new int[] {  2804,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 7, Suffix = "of the Bear",            Stat = new int[] {  2803,  2805,     0,     0,     0 }, Multiplier = new int[] { 10000,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 8, Suffix = "of the Whale",           Stat = new int[] {  2806,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666, 10000,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 9, Suffix = "of the Owl",             Stat = new int[] {  2804,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 10, Suffix = "of the Gorilla",        Stat = new int[] {  2804,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 11, Suffix = "of the Falcon",         Stat = new int[] {  2802,  2804,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 12, Suffix = "of the Boar",           Stat = new int[] {  2806,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 13, Suffix = "of the Wolf",           Stat = new int[] {  2802,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 14, Suffix = "of the Tiger",          Stat = new int[] {  2802,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 15, Suffix = "of Spirit",             Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 16, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 15000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 17, Suffix = "of Strength",           Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 18, Suffix = "of Agility",            Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 19, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 20, Suffix = "of Power",              Stat = new int[] {  2825,     0,     0,     0,     0 }, Multiplier = new int[] { 20000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 21, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 22, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 23, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 24, Suffix = "of Intellect",          Stat = new int[] {  2824,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 25, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 26, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 27, Suffix = "of Nimbleness",         Stat = new int[] {  2815,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 28, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 15000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 29, Suffix = "of Eluding",            Stat = new int[] {  2815,  2802,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 30, Suffix = "of Spirit",             Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 36, Suffix = "of the Sorcerer",       Stat = new int[] {  2803,  2804,  3726,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 37, Suffix = "of the Seer",           Stat = new int[] {  2803,  2804,  2822,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 38, Suffix = "of the Prophet",        Stat = new int[] {  2804,  2806,  3726,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 39, Suffix = "of the Invoker",        Stat = new int[] {  2804,  2822,     0,     0,     0 }, Multiplier = new int[] {  7889,  5259,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 40, Suffix = "of the Bandit",         Stat = new int[] {  2802,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 41, Suffix = "of the Beast",          Stat = new int[] {  3727,  2822,  2803,     0,     0 }, Multiplier = new int[] {  5259,  5259,  7889,     0,     0 } },
new RandomSuffixDataType() { Id = 42, Suffix = "of the Elder",          Stat = new int[] {  2803,  2806,  2804,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 43, Suffix = "of the Soldier",        Stat = new int[] {  2805,  2803,  2823,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 44, Suffix = "of the Elder",          Stat = new int[] {  2803,  2804,  2806,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 45, Suffix = "of the Champion",       Stat = new int[] {  2805,  2803,  2815,     0,     0 }, Multiplier = new int[] {  5259,  7889,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 47, Suffix = "of Blocking",           Stat = new int[] {  2826,  2805,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 49, Suffix = "of the Grove",          Stat = new int[] {  2805,  2802,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 50, Suffix = "of the Hunt",           Stat = new int[] {  2825,  2802,  2804,     0,     0 }, Multiplier = new int[] { 14532,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 51, Suffix = "of the Mind",           Stat = new int[] {  2824,  2822,  2804,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 52, Suffix = "of the Crusade",        Stat = new int[] {  2824,  2804,  2813,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 53, Suffix = "of the Vision",         Stat = new int[] {  2824,  2804,  2803,     0,     0 }, Multiplier = new int[] {  8501,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 54, Suffix = "of the Ancestor",       Stat = new int[] {  2805,  2823,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 56, Suffix = "of the Battle",         Stat = new int[] {  2805,  2803,  2823,     0,     0 }, Multiplier = new int[] {  7266,  6159,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 57, Suffix = "of the Shadow",         Stat = new int[] {  2825,  2802,  2803,     0,     0 }, Multiplier = new int[] { 14532,  4106,  4790,     0,     0 } },
new RandomSuffixDataType() { Id = 58, Suffix = "of the Sun",            Stat = new int[] {  2823,  2803,  2804,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 59, Suffix = "of the Moon",           Stat = new int[] {  2804,  2803,  2806,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 60, Suffix = "of the Wild",           Stat = new int[] {  2825,  2803,  2802,     0,     0 }, Multiplier = new int[] { 10518,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 61, Suffix = "of Intellect",          Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] {  2273,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 62, Suffix = "of Strength",           Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] {  5000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 63, Suffix = "of Agility",            Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] {  5000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 64, Suffix = "of Power",              Stat = new int[] {  2825,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 65, Suffix = "of Magic",              Stat = new int[] {  2824,     0,     0,     0,     0 }, Multiplier = new int[] {  5850,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 66, Suffix = "of the Knight",         Stat = new int[] {  2803,  2813,  2824,     0,     0 }, Multiplier = new int[] {  7889,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 67, Suffix = "of the Seer",           Stat = new int[] {  2803,  2822,  2804,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 68, Suffix = "of the Bear",           Stat = new int[] {  2805,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 69, Suffix = "of the Eagle",          Stat = new int[] {  2803,  2804,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 70, Suffix = "of the Ancestor",       Stat = new int[] {  2805,  2822,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 71, Suffix = "of the Bandit",         Stat = new int[] {  2802,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 72, Suffix = "of the Battle",         Stat = new int[] {  2805,  2803,  2822,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 73, Suffix = "of the Elder",          Stat = new int[] {  2803,  2804,  2806,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 74, Suffix = "of the Beast",          Stat = new int[] {  3727,  2822,  2803,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 75, Suffix = "of the Champion",       Stat = new int[] {  2805,  2803,  2815,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 76, Suffix = "of the Grove",          Stat = new int[] {  2805,  2802,  2803,     0,     0 }, Multiplier = new int[] {  7266,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 77, Suffix = "of the Knight",         Stat = new int[] {  2803,  2813,  2824,     0,     0 }, Multiplier = new int[] {  5259,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 78, Suffix = "of the Monkey",         Stat = new int[] {  2802,  2803,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 79, Suffix = "of the Moon",           Stat = new int[] {  2804,  2803,  2806,     0,     0 }, Multiplier = new int[] {  5259,  3506,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 80, Suffix = "of the Wild",           Stat = new int[] {  2825,  2803,  2802,     0,     0 }, Multiplier = new int[] { 10518,  3506,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 81, Suffix = "of the Whale",          Stat = new int[] {  2803,  2806,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 82, Suffix = "of the Vision",         Stat = new int[] {  2824,  2804,  2803,     0,     0 }, Multiplier = new int[] {  8501,  4106,  2129,     0,     0 } },
new RandomSuffixDataType() { Id = 83, Suffix = "of the Sun",            Stat = new int[] {  2823,  2803,  2804,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 84, Suffix = "of Stamina",            Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 85, Suffix = "of the Sorcerer",       Stat = new int[] {  2803,  2804,  3726,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 86, Suffix = "of the Soldier",        Stat = new int[] {  2805,  2803,  2822,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 87, Suffix = "of the Shadow",         Stat = new int[] {  2825,  2802,  2803,     0,     0 }, Multiplier = new int[] { 14532,  4106,  3193,     0,     0 } },
new RandomSuffixDataType() { Id = 88, Suffix = "of the Foreseer",       Stat = new int[] {  2804,  3726,  2824,     0,     0 }, Multiplier = new int[] {  5259,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 89, Suffix = "of the Thief",          Stat = new int[] {  2803,  2825,  3726,     0,     0 }, Multiplier = new int[] {  7889, 10518,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 90, Suffix = "of the Necromancer",    Stat = new int[] {  2803,  3727,  2824,     0,     0 }, Multiplier = new int[] {  7889,  5259,  6153,     0,     0 } },
new RandomSuffixDataType() { Id = 91, Suffix = "of the Marksman",       Stat = new int[] {  2803,  2802,  3727,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 92, Suffix = "of the Squire",         Stat = new int[] {  2803,  3727,  2805,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 93, Suffix = "of Restoration",        Stat = new int[] {  2803,  2824,  2816,     0,     0 }, Multiplier = new int[] {  7889,  6153,  2103,     0,     0 } },
new RandomSuffixDataType() { Id = 94, Suffix = "",                      Stat = new int[] {  2802,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 95, Suffix = "",                      Stat = new int[] {  2805,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 96, Suffix = "",                      Stat = new int[] {  2803,     0,     0,     0,     0 }, Multiplier = new int[] { 13500,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 97, Suffix = "",                      Stat = new int[] {  2804,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 98, Suffix = "",                      Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] {  9000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 99, Suffix = "of Speed",              Stat = new int[] {  3726,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 100, Suffix = "of the Principle",     Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 101, Suffix = "of the Sentinel",      Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 102, Suffix = "of the Hero",          Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 103, Suffix = "of the Avatar",        Stat = new int[] {  2803,  2805,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 104, Suffix = "of the Embodiment",    Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 105, Suffix = "of the Guardian",      Stat = new int[] {  2803,  2805,  4059,  2826,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 106, Suffix = "of the Defender",      Stat = new int[] {  2803,  2805,  2826,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 107, Suffix = "of the Exemplar",      Stat = new int[] {  2803,  2805,  4058,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 108, Suffix = "of the Curator",       Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 109, Suffix = "of the Preserver",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 110, Suffix = "of the Elements",      Stat = new int[] {  2803,  2804,  3727,  2823,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 111, Suffix = "of the Paradigm",      Stat = new int[] {  2803,  2804,  3727,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 112, Suffix = "of the Pattern",       Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 113, Suffix = "of the Essence",       Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 114, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 115, Suffix = "of the Archetype",     Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 116, Suffix = "of the Manifestation", Stat = new int[] {  2803,  2802,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 117, Suffix = "of the Incarnation",   Stat = new int[] {  2803,  2802,  2823,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 118, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 119, Suffix = "of the Ideal",         Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 120, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 121, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 122, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 123, Suffix = "of the Earthbreaker",  Stat = new int[] {  2803,  2805,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 124, Suffix = "of the Mountainbed",   Stat = new int[] {  2803,  2805,  4059,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 125, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 126, Suffix = "of the Substratum",    Stat = new int[] {  2803,  2805,  4058,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 127, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 128, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 129, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 130, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 131, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 132, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 133, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 134, Suffix = "of the Galeburst",     Stat = new int[] {  2803,  2802,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 135, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 136, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 137, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 138, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } },
new RandomSuffixDataType() { Id = 139, Suffix = "of the Mercenary",     Stat = new int[] {  2803,  2805,  3726,     0,     0 }, Multiplier = new int[] {  7889,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 140, Suffix = "of the Wraith",        Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 141, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 142, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 143, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 144, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 145, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 146, Suffix = "Crit/Mastery (40/60)", Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 147, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 148, Suffix = "Crit/Spirit (40/60)",  Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 149, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 150, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 151, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 152, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 153, Suffix = "of the Wraith",        Stat = new int[] {  3726,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 154, Suffix = "of the Shark",         Stat = new int[] {  2822,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 155, Suffix = "of the Scorpion",      Stat = new int[] {  3726,  2822,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 156, Suffix = "of the Wraith",        Stat = new int[] {  2822,  2806,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 157, Suffix = "of the Panther",       Stat = new int[] {  4059,  3726,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 158, Suffix = "of the Wind",          Stat = new int[] {  2806,  3726,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 159, Suffix = "of the Master",        Stat = new int[] {  2806,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 160, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  3726,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 161, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 162, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3506,  3506,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 163, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  3726,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 164, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 165, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  2664,  3997,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 166, Suffix = "of the Mongoose",      Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 167, Suffix = "of Storms",            Stat = new int[] {  3727,  2822,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 168, Suffix = "of Flames",            Stat = new int[] {  3727,  4059,     0,     0,     0 }, Multiplier = new int[] {  3997,  2664,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 169, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 170, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 171, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 172, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 173, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 174, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 175, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 176, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 177, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 178, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 179, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 180, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 181, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 182, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 183, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 184, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 185, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 186, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 187, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 188, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 189, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 190, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 191, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 192, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 193, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 194, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 195, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 196, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 197, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 198, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4638,  3205,  3205,     0 } },
new RandomSuffixDataType() { Id = 199, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 200, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 201, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 202, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4707,  3247,  3247,     0 } },
new RandomSuffixDataType() { Id = 203, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 204, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 205, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 206, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 207, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 208, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 209, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 210, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 211, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 212, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 213, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 214, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 215, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 216, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 217, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 218, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 219, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4834,  3320,  3320,     0 } },
new RandomSuffixDataType() { Id = 220, Suffix = "of the Earthshaker",   Stat = new int[] {  2803,  2805,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 221, Suffix = "of the Landslide",     Stat = new int[] {  2803,  2805,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 222, Suffix = "of the Earthfall",     Stat = new int[] {  2803,  2805,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 223, Suffix = "of the Faultline",     Stat = new int[] {  2803,  2805,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 224, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 225, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 226, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 227, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 228, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 229, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 230, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 231, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 232, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 233, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 234, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 235, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } },
new RandomSuffixDataType() { Id = 236, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4890,  3327,  3327,     0 } }, 
new RandomSuffixDataType() { Id = 260, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 261, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 262, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 263, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 264, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 265, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 266, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 267, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 268, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 269, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 270, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4490,  3125,  3125,     0 } },
new RandomSuffixDataType() { Id = 271, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4580,  3165,  3165,     0 } },
new RandomSuffixDataType() { Id = 272, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 273, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 274, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 275, Suffix = "of the Bedrock",       Stat = new int[] {  2803,  2805,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 276, Suffix = "of the Bouldercrag",   Stat = new int[] {  2803,  2805,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 277, Suffix = "of the Rockslab",      Stat = new int[] {  2803,  2805,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 278, Suffix = "of the Wildfire",      Stat = new int[] {  2803,  2804,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 279, Suffix = "of the Flameblaze",    Stat = new int[] {  2803,  2804,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 280, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 281, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 282, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 283, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 284, Suffix = "of the Fireflash",     Stat = new int[] {  2803,  2804,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 285, Suffix = "of the Feverflare",    Stat = new int[] {  2803,  2804,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 286, Suffix = "of the Undertow",      Stat = new int[] {  2803,  2804,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 287, Suffix = "of the Wavecrest",     Stat = new int[] {  2803,  2804,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 288, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 289, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 290, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 291, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4900,  3340,  3340,     0 } },
new RandomSuffixDataType() { Id = 292, Suffix = "of the Stormblast",    Stat = new int[] {  2803,  2802,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 293, Suffix = "of the Windflurry",    Stat = new int[] {  2803,  2802,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 294, Suffix = "of the Windstorm",     Stat = new int[] {  2803,  2802,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 295, Suffix = "of the Zephyr",        Stat = new int[] {  2803,  2802,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  4950,  3360,  3360,     0 } },
new RandomSuffixDataType() { Id = 296, Suffix = "TMP of Hit",           Stat = new int[] {  3727,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 297, Suffix = "TMP of Crit",          Stat = new int[] {  2822,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 298, Suffix = "TMP of Str/Hit",       Stat = new int[] {  2805,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 299, Suffix = "TMP of Agi/Hit",       Stat = new int[] {  2802,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 300, Suffix = "TMP of Int/Hit",       Stat = new int[] {  2804,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 301, Suffix = "TMP of Stam/Hit",      Stat = new int[] {  2803,  3727,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 302, Suffix = "TMP of Str/Crit",      Stat = new int[] {  2805,  2822,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 303, Suffix = "TMP of Agi/Crit",      Stat = new int[] {  2802,  2822,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 304, Suffix = "TMP of Stam/Crit",     Stat = new int[] {  2803,  2822,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 305, Suffix = "TMP of Str/Dodge",     Stat = new int[] {  2805,  2815,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 306, Suffix = "TMP of Str/Haste",     Stat = new int[] {  2805,  3726,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 307, Suffix = "TMP of Agi/Haste",     Stat = new int[] {  2802,  3726,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 308, Suffix = "TMP of Int/Haste",     Stat = new int[] {  2804,  3726,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 309, Suffix = "TMP of Stam/Haste",    Stat = new int[] {  2803,  3726,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 310, Suffix = "TMP of Parry",         Stat = new int[] {  4060,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 311, Suffix = "TMP of Expertise",     Stat = new int[] {  4058,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 312, Suffix = "TMP of Str/Exp",       Stat = new int[] {  2805,  4058,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 313, Suffix = "TMP of Dodge/Parry",   Stat = new int[] {  2815,  4060,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 314, Suffix = "TMP of Agi/Exp",       Stat = new int[] {  2802,  4058,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 315, Suffix = "TMP of Stam/Exp",      Stat = new int[] {  2803,  4058,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 316, Suffix = "TMP of Str/Parry",     Stat = new int[] {  2805,  4060,     0,     0,     0 }, Multiplier = new int[] {  6666,  6666,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 325, Suffix = "of Dodge",             Stat = new int[] {  2815,     0,     0,     0,     0 }, Multiplier = new int[] { 10000,     0,     0,     0,     0 } },
new RandomSuffixDataType() { Id = 330, Suffix = "of Restoration",       Stat = new int[] {  2803,  2804,  2806,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 331, Suffix = "of Striking",          Stat = new int[] {  2803,  2825,  3727,     0,     0 }, Multiplier = new int[] {  5259, 10518,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 332, Suffix = "of Sorcery",           Stat = new int[] {  2803,  2804,  3727,     0,     0 }, Multiplier = new int[] {  5259,  5259,  5259,     0,     0 } },
new RandomSuffixDataType() { Id = 336, Suffix = "",                     Stat = new int[] {  2822,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 337, Suffix = "",                     Stat = new int[] {  3727,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 338, Suffix = "",                     Stat = new int[] {  4058,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 339, Suffix = "",                     Stat = new int[] {  4059,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 340, Suffix = "",                     Stat = new int[] {  3726,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 341, Suffix = "",                     Stat = new int[] {  4060,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 342, Suffix = "",                     Stat = new int[] {  2815,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 343, Suffix = "",                     Stat = new int[] {  2806,  0,     0,        0,     0 }, Multiplier = new int[] {  5200,  0,        0,     0,     0 } },
new RandomSuffixDataType() { Id = 344, Suffix = "of the Decimator",     Stat = new int[] {  2822,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Crit
new RandomSuffixDataType() { Id = 345, Suffix = "of the Unerring",      Stat = new int[] {  3727,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Hit
new RandomSuffixDataType() { Id = 346, Suffix = "of the Adroit",        Stat = new int[] {  4058,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Expertise
new RandomSuffixDataType() { Id = 347, Suffix = "of the Savant",        Stat = new int[] {  4059,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Mastery
new RandomSuffixDataType() { Id = 348, Suffix = "of the Impatient",     Stat = new int[] {  3726,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Haste
new RandomSuffixDataType() { Id = 349, Suffix = "of the Bladewall",		Stat = new int[] {  4060,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Parry
new RandomSuffixDataType() { Id = 350, Suffix = "of the Untouchable",   Stat = new int[] {  2815,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Dodge
new RandomSuffixDataType() { Id = 351, Suffix = "of the Pious",		    Stat = new int[] {  2806,     0,     0,     0,     0 }, Multiplier = new int[] {  5200,     0,     0,     0,     0 } }, // 5.3 Barrens - Spirit
new RandomSuffixDataType() { Id = 352, Suffix = "of the Landslide",		Stat = new int[] {     0,     0,  3727,  4058,     0 }, Multiplier = new int[] {     0,     0,  3506,  3506,     0 } }, // 5.3 Landslide - Str Versa
new RandomSuffixDataType() { Id = 353, Suffix = "of the Stormblast",	Stat = new int[] {     0,     0,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Stormblast - Agi
new RandomSuffixDataType() { Id = 354, Suffix = "of the Galeburst",		Stat = new int[] {     0,     0,  3727,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Galeburst - Agi
new RandomSuffixDataType() { Id = 355, Suffix = "of the Windflurry",	Stat = new int[] {     0,     0,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Windflurry - Agi
new RandomSuffixDataType() { Id = 356, Suffix = "of the Windstorm",		Stat = new int[] {     0,     0,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Windstorm - Agi
new RandomSuffixDataType() { Id = 357, Suffix = "of the Zephyr",		Stat = new int[] {     0,     0,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Zephyr - Agi
new RandomSuffixDataType() { Id = 358, Suffix = "of the Wildfire",		Stat = new int[] {     0,     0,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Wildfire - Int Hit
new RandomSuffixDataType() { Id = 359, Suffix = "of the Flameblaze",	Stat = new int[] {     0,     0,  4059,  3727,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Flameblaze - Int Hit
new RandomSuffixDataType() { Id = 360, Suffix = "of the Fireflash",		Stat = new int[] {     0,     0,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Fireflash - Int Versa
new RandomSuffixDataType() { Id = 361, Suffix = "of the Feverflare",	Stat = new int[] {     0,     0,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Feverflare - Int Versa
new RandomSuffixDataType() { Id = 362, Suffix = "of the Undertow",		Stat = new int[] {     0,     0,  3726,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Undertow - Int Spirit
new RandomSuffixDataType() { Id = 363, Suffix = "of the Wavecrest",		Stat = new int[] {     0,     0,  4059,  2806,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Wavecrest - Int Spirit
new RandomSuffixDataType() { Id = 364, Suffix = "of the Earthbreaker",	Stat = new int[] {     0,     0,  2822,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Earthbreaker - Str DPS
new RandomSuffixDataType() { Id = 365, Suffix = "of the Faultline",		Stat = new int[] {     0,     0,  3726,  4059,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Faultline - Str DPS
new RandomSuffixDataType() { Id = 366, Suffix = "of the Mountainbed",	Stat = new int[] {     0,     0,  4059,  4058,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Mountainbed - Str Vers
new RandomSuffixDataType() { Id = 367, Suffix = "of the Bedrock",		Stat = new int[] {     0,     0,  4059,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Bedrock - Str Tank
new RandomSuffixDataType() { Id = 368, Suffix = "of the Bouldercrag",	Stat = new int[] {     0,     0,  2815,  4060,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Bouldercrag - Str Tank
new RandomSuffixDataType() { Id = 369, Suffix = "of the Rockslab",		Stat = new int[] {     0,     0,  4059,  2815,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Rockslab - Str Tank
new RandomSuffixDataType() { Id = 370, Suffix = "of the Earthshaker",	Stat = new int[] {     0,     0,  3727,  2822,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Earthshaker - Str DPS
new RandomSuffixDataType() { Id = 371, Suffix = "of the Earthfall",		Stat = new int[] {     0,     0,  2822,  3726,     0 }, Multiplier = new int[] {  7889,  5259,  3506,  3506,     0 } }, // 5.3 Earthfall - Str DPS
new RandomSuffixDataType() { Id = 372, Suffix = "",		Stat = new int[] {     0,     0,  0,  0,     0 }, Multiplier = new int[] {  0,  0,  0,  0,     0 } }, // Filler
new RandomSuffixDataType() { Id = 373, Suffix = "",		Stat = new int[] {     0,     0,  0,  0,     0 }, Multiplier = new int[] {  0,  0,  0,  0,     0 } }, // Filler
new RandomSuffixDataType() { Id = 374, Suffix = "",		Stat = new int[] {     0,     0,  0,  0,     0 }, Multiplier = new int[] {  0,  0,  0,  0,     0 } }, // Filler
new RandomSuffixDataType() { Id = 375, Suffix = "",		Stat = new int[] {     0,     0,  0,  0,     0 }, Multiplier = new int[] {  0,  0,  0,  0,     0 } }, // Filler
new RandomSuffixDataType() { Id = 376, Suffix = "",		Stat = new int[] {     0,     0,  0,  0,     0 }, Multiplier = new int[] {  0,  0,  0,  0,     0 } }, // Filler
new RandomSuffixDataType() { Id = 377, Suffix = "of the Decimator",	Stat = new int[] {	2822,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Crit
new RandomSuffixDataType() { Id = 378, Suffix = "of the Unerring",	Stat = new int[] {	3727,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Hit
new RandomSuffixDataType() { Id = 379, Suffix = "of the Adroit",	Stat = new int[] {	4058,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Expertise
new RandomSuffixDataType() { Id = 380, Suffix = "of the Savant",	Stat = new int[] {	4059,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Mastery
new RandomSuffixDataType() { Id = 381, Suffix = "of the Impatient",	Stat = new int[] {	3726,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Haste
new RandomSuffixDataType() { Id = 382, Suffix = "of the Bladewall",	Stat = new int[] {	4060,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Parry
new RandomSuffixDataType() { Id = 383, Suffix = "of the Untouchable",	Stat = new int[] {	2815,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Dodge
new RandomSuffixDataType() { Id = 384, Suffix = "of the Pious",	Stat = new int[] {	2806,	0,	0,	0,	0 }, Multiplier = new int[] {	4170,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Spirit
new RandomSuffixDataType() { Id = 385, Suffix = "of the Landslide",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	0,	0,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Landslide - Str Versa
new RandomSuffixDataType() { Id = 386, Suffix = "of the Stormblast",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Stormblast - Agi
new RandomSuffixDataType() { Id = 387, Suffix = "of the Galeburst",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Galeburst - Agi
new RandomSuffixDataType() { Id = 388, Suffix = "of the Windflurry",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Windflurry - Agi
new RandomSuffixDataType() { Id = 389, Suffix = "of the Windstorm",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Windstorm - Agi
new RandomSuffixDataType() { Id = 390, Suffix = "of the Zephyr",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Zephyr - Agi
new RandomSuffixDataType() { Id = 391, Suffix = "of the Wildfire",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Wildfire - Int Hit
new RandomSuffixDataType() { Id = 392, Suffix = "of the Flameblaze",	Stat = new int[] {	0,	0,	4059,	3727,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Flameblaze - Int Hit
new RandomSuffixDataType() { Id = 393, Suffix = "of the Fireflash",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Fireflash - Int Versa
new RandomSuffixDataType() { Id = 394, Suffix = "of the Feverflare",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Feverflare - Int Versa
new RandomSuffixDataType() { Id = 395, Suffix = "of the Undertow",	Stat = new int[] {	0,	0,	3726,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Undertow - Int Spirit
new RandomSuffixDataType() { Id = 396, Suffix = "of the Wavecrest",	Stat = new int[] {	0,	0,	4059,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Wavecrest - Int Spirit
new RandomSuffixDataType() { Id = 397, Suffix = "of the Earthbreaker",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Earthbreaker - Str DPS
new RandomSuffixDataType() { Id = 398, Suffix = "of the Faultline",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Faultline - Str DPS
new RandomSuffixDataType() { Id = 399, Suffix = "of the Mountainbed",	Stat = new int[] {	0,	0,	4059,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Mountainbed - Str Vers
new RandomSuffixDataType() { Id = 400, Suffix = "of the Bedrock",	Stat = new int[] {	0,	0,	4059,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Bedrock - Str Tank
new RandomSuffixDataType() { Id = 401, Suffix = "of the Bouldercrag",	Stat = new int[] {	0,	0,	2815,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Bouldercrag - Str Tank
new RandomSuffixDataType() { Id = 402, Suffix = "of the Rockslab",	Stat = new int[] {	0,	0,	4059,	2815,	0 }, Multiplier = new int[] {	7889,	5259,	2990,	2990,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Rockslab - Str Tank
new RandomSuffixDataType() { Id = 403, Suffix = "of the Earthshaker",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3506,	3506,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Earthshaker - Str DPS
new RandomSuffixDataType() { Id = 404, Suffix = "of the Earthfall",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3506,	3506,	0 } }, // 5.4 (ilvl 496) - 1.5 socket cost - Earthfall - Str DPS
new RandomSuffixDataType() { Id = 405, Suffix = "of the Decimator",	Stat = new int[] {	2822,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Crit
new RandomSuffixDataType() { Id = 406, Suffix = "of the Unerring",	Stat = new int[] {	3727,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Hit
new RandomSuffixDataType() { Id = 407, Suffix = "of the Adroit",	Stat = new int[] {	4058,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Expertise
new RandomSuffixDataType() { Id = 408, Suffix = "of the Savant",	Stat = new int[] {	4059,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Mastery
new RandomSuffixDataType() { Id = 409, Suffix = "of the Impatient",	Stat = new int[] {	3726,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Haste
new RandomSuffixDataType() { Id = 410, Suffix = "of the Bladewall",	Stat = new int[] {	4060,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Parry
new RandomSuffixDataType() { Id = 411, Suffix = "of the Untouchable",	Stat = new int[] {	2815,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Dodge
new RandomSuffixDataType() { Id = 412, Suffix = "of the Pious",	Stat = new int[] {	2806,	0,	0,	0,	0 }, Multiplier = new int[] {	4960,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Spirit
new RandomSuffixDataType() { Id = 413, Suffix = "of the Landslide",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	0,	0,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Landslide - Str Versa
new RandomSuffixDataType() { Id = 414, Suffix = "of the Stormblast",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Stormblast - Agi
new RandomSuffixDataType() { Id = 415, Suffix = "of the Galeburst",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Galeburst - Agi
new RandomSuffixDataType() { Id = 416, Suffix = "of the Windflurry",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Windflurry - Agi
new RandomSuffixDataType() { Id = 417, Suffix = "of the Windstorm",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Windstorm - Agi
new RandomSuffixDataType() { Id = 418, Suffix = "of the Zephyr",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Zephyr - Agi
new RandomSuffixDataType() { Id = 419, Suffix = "of the Wildfire",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Wildfire - Int Hit
new RandomSuffixDataType() { Id = 420, Suffix = "of the Flameblaze",	Stat = new int[] {	0,	0,	4059,	3727,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Flameblaze - Int Hit
new RandomSuffixDataType() { Id = 421, Suffix = "of the Fireflash",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Fireflash - Int Versa
new RandomSuffixDataType() { Id = 422, Suffix = "of the Feverflare",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Feverflare - Int Versa
new RandomSuffixDataType() { Id = 423, Suffix = "of the Undertow",	Stat = new int[] {	0,	0,	3726,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Undertow - Int Spirit
new RandomSuffixDataType() { Id = 424, Suffix = "of the Wavecrest",	Stat = new int[] {	0,	0,	4059,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Wavecrest - Int Spirit
new RandomSuffixDataType() { Id = 425, Suffix = "of the Earthbreaker",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Earthbreaker - Str DPS
new RandomSuffixDataType() { Id = 426, Suffix = "of the Faultline",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Faultline - Str DPS
new RandomSuffixDataType() { Id = 427, Suffix = "of the Mountainbed",	Stat = new int[] {	0,	0,	4059,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Mountainbed - Str Vers
new RandomSuffixDataType() { Id = 428, Suffix = "of the Bedrock",	Stat = new int[] {	0,	0,	4059,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Bedrock - Str Tank
new RandomSuffixDataType() { Id = 429, Suffix = "of the Bouldercrag",	Stat = new int[] {	0,	0,	2815,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Bouldercrag - Str Tank
new RandomSuffixDataType() { Id = 430, Suffix = "of the Rockslab",	Stat = new int[] {	0,	0,	4059,	2815,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Rockslab - Str Tank
new RandomSuffixDataType() { Id = 431, Suffix = "of the Earthshaker",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Earthshaker - Str DPS
new RandomSuffixDataType() { Id = 432, Suffix = "of the Earthfall",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3268,	3268,	0 } }, // 5.4 (ilvl 535) - 0.5 socket cost - Earthfall - Str DPS
new RandomSuffixDataType() { Id = 433, Suffix = "of the Decimator",	Stat = new int[] {	2822,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Crit
new RandomSuffixDataType() { Id = 434, Suffix = "of the Unerring",	Stat = new int[] {	3727,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Hit
new RandomSuffixDataType() { Id = 435, Suffix = "of the Adroit",	Stat = new int[] {	4058,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Expertise
new RandomSuffixDataType() { Id = 436, Suffix = "of the Savant",	Stat = new int[] {	4059,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Mastery
new RandomSuffixDataType() { Id = 437, Suffix = "of the Impatient",	Stat = new int[] {	3726,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Haste
new RandomSuffixDataType() { Id = 438, Suffix = "of the Bladewall",	Stat = new int[] {	4060,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Parry
new RandomSuffixDataType() { Id = 439, Suffix = "of the Untouchable",	Stat = new int[] {	2815,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Dodge
new RandomSuffixDataType() { Id = 440, Suffix = "of the Pious",	Stat = new int[] {	2806,	0,	0,	0,	0 }, Multiplier = new int[] {	4480,	0,	0,	0,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Spirit
new RandomSuffixDataType() { Id = 441, Suffix = "of the Landslide",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	0,	0,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Landslide - Str Versa
new RandomSuffixDataType() { Id = 442, Suffix = "of the Stormblast",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Stormblast - Agi
new RandomSuffixDataType() { Id = 443, Suffix = "of the Galeburst",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Galeburst - Agi
new RandomSuffixDataType() { Id = 444, Suffix = "of the Windflurry",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Windflurry - Agi
new RandomSuffixDataType() { Id = 445, Suffix = "of the Windstorm",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Windstorm - Agi
new RandomSuffixDataType() { Id = 446, Suffix = "of the Zephyr",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Zephyr - Agi
new RandomSuffixDataType() { Id = 447, Suffix = "of the Wildfire",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Wildfire - Int Hit
new RandomSuffixDataType() { Id = 448, Suffix = "of the Flameblaze",	Stat = new int[] {	0,	0,	4059,	3727,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Flameblaze - Int Hit
new RandomSuffixDataType() { Id = 449, Suffix = "of the Fireflash",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Fireflash - Int Versa
new RandomSuffixDataType() { Id = 450, Suffix = "of the Feverflare",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Feverflare - Int Versa
new RandomSuffixDataType() { Id = 451, Suffix = "of the Undertow",	Stat = new int[] {	0,	0,	3726,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Undertow - Int Spirit
new RandomSuffixDataType() { Id = 452, Suffix = "of the Wavecrest",	Stat = new int[] {	0,	0,	4059,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Wavecrest - Int Spirit
new RandomSuffixDataType() { Id = 453, Suffix = "of the Earthbreaker",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Earthbreaker - Str DPS
new RandomSuffixDataType() { Id = 454, Suffix = "of the Faultline",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Faultline - Str DPS
new RandomSuffixDataType() { Id = 455, Suffix = "of the Mountainbed",	Stat = new int[] {	0,	0,	4059,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Mountainbed - Str Vers
new RandomSuffixDataType() { Id = 456, Suffix = "of the Bedrock",	Stat = new int[] {	0,	0,	4059,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Bedrock - Str Tank
new RandomSuffixDataType() { Id = 457, Suffix = "of the Bouldercrag",	Stat = new int[] {	0,	0,	2815,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Bouldercrag - Str Tank
new RandomSuffixDataType() { Id = 458, Suffix = "of the Rockslab",	Stat = new int[] {	0,	0,	4059,	2815,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Rockslab - Str Tank
new RandomSuffixDataType() { Id = 459, Suffix = "of the Earthshaker",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Earthshaker - Str DPS
new RandomSuffixDataType() { Id = 460, Suffix = "of the Earthfall",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	2788,	2788,	0 } }, // 5.4 (ilvl 535) - 1.5 socket cost - Earthfall - Str DPS
new RandomSuffixDataType() { Id = 461, Suffix = "of the Decimator",	Stat = new int[] {	2822,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Crit
new RandomSuffixDataType() { Id = 462, Suffix = "of the Unerring",	Stat = new int[] {	3727,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Hit
new RandomSuffixDataType() { Id = 463, Suffix = "of the Adroit",	Stat = new int[] {	4058,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Expertise
new RandomSuffixDataType() { Id = 464, Suffix = "of the Savant",	Stat = new int[] {	4059,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Mastery
new RandomSuffixDataType() { Id = 465, Suffix = "of the Impatient",	Stat = new int[] {	3726,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Haste
new RandomSuffixDataType() { Id = 466, Suffix = "of the Bladewall",	Stat = new int[] {	4060,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Parry
new RandomSuffixDataType() { Id = 467, Suffix = "of the Untouchable",	Stat = new int[] {	2815,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Dodge
new RandomSuffixDataType() { Id = 468, Suffix = "of the Pious",	Stat = new int[] {	2806,	0,	0,	0,	0 }, Multiplier = new int[] {	4859,	0,	0,	0,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Spirit
new RandomSuffixDataType() { Id = 469, Suffix = "of the Landslide",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	0,	0,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Landslide - Str Versa
new RandomSuffixDataType() { Id = 470, Suffix = "of the Stormblast",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Stormblast - Agi
new RandomSuffixDataType() { Id = 471, Suffix = "of the Galeburst",	Stat = new int[] {	0,	0,	3727,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Galeburst - Agi
new RandomSuffixDataType() { Id = 472, Suffix = "of the Windflurry",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Windflurry - Agi
new RandomSuffixDataType() { Id = 473, Suffix = "of the Windstorm",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Windstorm - Agi
new RandomSuffixDataType() { Id = 474, Suffix = "of the Zephyr",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Zephyr - Agi
new RandomSuffixDataType() { Id = 475, Suffix = "of the Wildfire",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Wildfire - Int Hit
new RandomSuffixDataType() { Id = 476, Suffix = "of the Flameblaze",	Stat = new int[] {	0,	0,	4059,	3727,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Flameblaze - Int Hit
new RandomSuffixDataType() { Id = 477, Suffix = "of the Fireflash",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Fireflash - Int Versa
new RandomSuffixDataType() { Id = 478, Suffix = "of the Feverflare",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Feverflare - Int Versa
new RandomSuffixDataType() { Id = 479, Suffix = "of the Undertow",	Stat = new int[] {	0,	0,	3726,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Undertow - Int Spirit
new RandomSuffixDataType() { Id = 480, Suffix = "of the Wavecrest",	Stat = new int[] {	0,	0,	4059,	2806,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Wavecrest - Int Spirit
new RandomSuffixDataType() { Id = 481, Suffix = "of the Earthbreaker",	Stat = new int[] {	0,	0,	2822,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Earthbreaker - Str DPS
new RandomSuffixDataType() { Id = 482, Suffix = "of the Faultline",	Stat = new int[] {	0,	0,	3726,	4059,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Faultline - Str DPS
new RandomSuffixDataType() { Id = 483, Suffix = "of the Mountainbed",	Stat = new int[] {	0,	0,	4059,	4058,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Mountainbed - Str Vers
new RandomSuffixDataType() { Id = 484, Suffix = "of the Bedrock",	Stat = new int[] {	0,	0,	4059,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Bedrock - Str Tank
new RandomSuffixDataType() { Id = 485, Suffix = "of the Bouldercrag",	Stat = new int[] {	0,	0,	2815,	4060,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Bouldercrag - Str Tank
new RandomSuffixDataType() { Id = 486, Suffix = "of the Rockslab",	Stat = new int[] {	0,	0,	4059,	2815,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Rockslab - Str Tank
new RandomSuffixDataType() { Id = 487, Suffix = "of the Earthshaker",	Stat = new int[] {	0,	0,	3727,	2822,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Earthshaker - Str DPS
new RandomSuffixDataType() { Id = 488, Suffix = "of the Earthfall",	Stat = new int[] {	0,	0,	2822,	3726,	0 }, Multiplier = new int[] {	7889,	5259,	3159,	3159,	0 } }, // 5.4 (ilvl 496) - 0.5 socket cost - Earthfall - Str DPS
        });
        private static List<int> allSuffixes;

        public static List<int> GetAllSuffixes()
        {
            if (allSuffixes == null)
            {
                List<int> list = new List<int>();
                foreach (var s in RandomSuffixData)
                {
                    if (s != null)
                    {
                        list.Add(s.Id);
                    }
                }
                allSuffixes = list;
            }
            return allSuffixes;
        }

        /// <summary>
        /// Gets the number of random stats.
        /// </summary>
        public static int GetStatCount(Item item, int id)
        {
            for (int i = 0; i < 5; i++)
            {
                if (RandomSuffixData[id].Stat[i] == 0)
                {
                    return i;
                }
            }
            return 5;
        }

        /// <summary>
        /// Gets the type of random stat at given index.
        /// </summary>
        public static AdditiveStat GetStat(Item item, int id, int index)
        {
            return StatFromEnchantmentId(RandomSuffixData[id].Stat[index]);
        }

        /// <summary>
        /// Gets the ammount of random stat at a given index.
        /// </summary>
        public static float GetStatValue(Item item, int id, int index)
        {
            if (item.ItemLevel < 277) return 0;
            int baseValue = ItemInstance.GetItemBudget(item, 0);
            int multiplier = RandomSuffixData[id].Multiplier[index];
            return (int)(multiplier / 10000.0 * baseValue);
        }

        /// <summary>
        /// Gets the ammount of random stat for a given stat.
        /// </summary>
        public static float GetStatValue(Item item, int id, AdditiveStat stat)
        {
            if (item.ItemLevel < 277) return 0;
            for (int i = 0; i < 5; i++)
            {
                int statId = RandomSuffixData[id].Stat[i];
                if (statId == 0)
                {
                    continue;
                }
                if (stat == StatFromEnchantmentId(statId))
                {
					int baseValue = ItemInstance.GetItemBudget(item, 0);
                    int multiplier = RandomSuffixData[id].Multiplier[i];
                    return (int)(multiplier / 10000.0 * baseValue);
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets the suffix for given id.
        /// </summary>
        public static string GetSuffix(int id)
        {
            if (id < RandomSuffixData.Length)
                return RandomSuffixData[id].Suffix;
            else
                return "";
        }

        public static void AccumulateStats(Stats stats, Item item, int id)
        {
            //&UT& 
            // Should check ID to ensure it's not outside the array.
            if (item.ItemLevel < 277 
                || id < 0
                || id > RandomSuffixData.Length) return;
            for (int i = 0; i < 5; i++)
            {
                int statId = RandomSuffixData[id].Stat[i];
                if (statId == 0)
                {
                    continue;
                }
                AdditiveStat stat = StatFromEnchantmentId(statId);
				int baseValue = ItemInstance.GetItemBudget(item, 0);
                int multiplier = RandomSuffixData[id].Multiplier[i];
                stats._rawAdditiveData[(int)stat] += (int)(multiplier / 10000.0 * baseValue);
            }
        }

		public static void AccumulateUpgradeStatsDBC(Stats stats, int randomSuffixId, int itemBudget)
		{
			for (int i = 0; i < 5; i++)
			{
				int statId = RandomSuffixData[randomSuffixId].Stat[i];
				if (statId == 0)
				{
					continue;
				}
				AdditiveStat stat = StatFromEnchantmentId(statId);
				int multiplier = RandomSuffixData[randomSuffixId].Multiplier[i];
				stats._rawAdditiveData[(int)stat] = (int)(multiplier / 10000.0 * itemBudget);				
			}
		}

        private static AdditiveStat StatFromEnchantmentId(int id)
        {
            switch (id)
            {
                case 2802:
                    return AdditiveStat.Agility;
                case 2803:
                    return AdditiveStat.Stamina;
                case 2804:
                    return AdditiveStat.Intellect;
                case 2805:
                    return AdditiveStat.Strength;
                case 2806:
                    return AdditiveStat.Spirit;
                case 2813:
                case 2815:
                    return AdditiveStat.DodgeRating;
                case 2816:
                    return AdditiveStat.Mp5;
                case 2822:
                case 2823:
                    return AdditiveStat.CritRating;
                case 2824:
                    return AdditiveStat.SpellPower;
                case 2825:
                    return AdditiveStat.AttackPower;
                case 2826:
                    return AdditiveStat.BlockRating;
                case 3726:
                    return AdditiveStat.HasteRating;
                case 3727:
                    return AdditiveStat.HitRating;
                case 4058:
                    return AdditiveStat.ExpertiseRating;
                case 4059:
                    return AdditiveStat.MasteryRating;
                case 4060:
                    return AdditiveStat.ParryRating;
            }
            return (AdditiveStat)(-1);
        }
    }
}
