using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace productboard.Models
{
    /// <summary>
    /// A model for creating notes on productboard
    /// </summary>
    public class Note
    {
        /// <example>Note title</example>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// HTML-encoded rich text supporting only certain tags; unsupported tags will be stripped out
        /// </summary>
        /// <example>Here is some <b>exciting</b> content</example>
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// The email address of customer to attach to the note - will use an
        /// existing customer record if one is found, otherwise will create one
        /// with the specified email address within a company with matching
        /// domain (if it already exists).
        /// </summary>
        /// <example>customer@example.com</example>
        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }

        /// <summary>
        /// For entities that originated in external systems and entered
        /// productboard via the API or integrations, a url where the external
        /// entity can be accessed - displayed as a clickable title in the
        /// productboard UI.
        /// </summary>
        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; set; }

        /// <summary>
        /// For entities that originated in external systems and entered
        /// productboard via the API or integrations, the source keeps track of the
        /// original source entity in that origin system(s)
        /// </summary>
        [JsonPropertyName("source")]
        public Source Source { get; set; }

        /// <summary>
        /// A set of tags for categorizing the note; tag uniqueness is case-and diacritic-insensitive,
        /// so <c>Apple</c>, <c>APPLE</c>, and <c>äpple</c> will all end up assigned to the same tag, and the tag displayed
        /// will be whichever variant was first(chronologically) entered into productboard.
        /// </summary>
        /// <example>
        /// <list type="bullet">
        /// <item>3.0</item>
        /// <item>important</item>
        /// <item>experimental</item>
        /// </list>
        /// </example>
        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }
    }
}
