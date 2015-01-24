using System;
using System.Collections.Generic;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using OpenTable.Features.Core;
using Shouldly;

namespace Core_Tests
{
// ReSharper disable InconsistentNaming
    public class SitePhrasesTests
    {
        private static string build_phrases_json(Func<PhrasesDatabaseJsonBuilder, PhrasesDatabaseJsonBuilder> func)
        {
            return func(new PhrasesDatabaseJsonBuilder()).Build();
        }

        [TestFixture]
        public class when_the_specified_domain_is_known
        {
            private SitePhrases subject;
            private const string known_phrase = "three blind mice";
            private const int dot_com_domain_id = 1;

            [SetUp]
            public void because()
            {
                subject = new SitePhrases(dot_com_domain_id, build_phrases_json(p => p
                        .add_key("ExampleKey", d => d["com"] = known_phrase)
                        ));
            }

            [Test]
            public void it_should_have_example_key()
            {
                subject.GetText("ExampleKey").ShouldBe(known_phrase);
            }
        }

        [TestFixture]
        [Ignore("PC:20130213 - wip")]
        public class when_the_key_exists_but_not_for_specified_domain
        {
            private SitePhrases subject;
            private const string known_phrase = "see how they run";
            private const int unknown_domain_id = 456769;

            [SetUp]
            public void because()
            {
                subject = new SitePhrases(unknown_domain_id, build_phrases_json(p => p
                        .add_key("ExampleKey", d =>
                            {
                                d["some_domain_identifier"] = known_phrase;
                                d["com"] = "run, Forest, run!";
                            })));
            }

            [Test]
            public void it_should_use_first_dictionary_entry()
            {
                subject.GetText("ExampleKey").ShouldBe(known_phrase);
            }
        }
    }

    internal class PhrasesDatabaseJsonBuilder
    {
        private readonly Dictionary<string, Dictionary<string, string>> _dictionaryDatabase;

        public PhrasesDatabaseJsonBuilder()
        {
            _dictionaryDatabase = new Dictionary<string, Dictionary<string, string>>();
        }

        public string Build()
        {
            return JObject.FromObject(_dictionaryDatabase).ToString();
        }

        public PhrasesDatabaseJsonBuilder add_key(string key, Action<Dictionary<string, string>> func)
        {
            var dictionary = new Dictionary<string, string>();
            func(dictionary);
            _dictionaryDatabase[key] = dictionary;

            return this;
        }
    }

// ReSharper restore InconsistentNaming
}
