using System;
using System.ComponentModel.DataAnnotations;
using Orchard.ContentManagement;

namespace Orchard.Disqus.Models
{
    public class DisqusSettingsPart : ContentPart<DisqusSettingsRecord>
    {
        [Required]
        public string ShortName
        {
            get { return Record.ShortName; }
            set { Record.ShortName = value; }
        }

        [Required]
        public string ApiKey
        {
            get { return Record.ApiKey; }
            set { Record.ApiKey = value; }
        }

        public DateTime? SynchronizedAt
        {
            get { return Record.SynchronizedAt; }
            set { Record.SynchronizedAt = value; }
        }

        public bool IsValid()
        {
            return !String.IsNullOrEmpty(ApiKey) && !String.IsNullOrEmpty(ShortName);
        }
    }
}