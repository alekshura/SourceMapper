using Compentio.SourceMapper.Attributes;
using System;

namespace Compentio.Example.DotNetCore.App.Entities
{
    public class NoteDao
    {
        public long Id { get; set; }
        public string PageTitle { get; set; }
        public string Description { get; set; }

        [IgnoreMapping]
        public DateTime ValidFrom { get; set; }

        [IgnoreMapping]
        public DateTime ValidTo { get; set; }

        [IgnoreMapping]
        public string CreatedBy { get; set; }

        [IgnoreMapping]
        public DateTime Created { get; set; }

        [IgnoreMapping]
        public DateTime Modified { get; set; }

        public NoteDocumentDao Document { get; set; }
    }
}