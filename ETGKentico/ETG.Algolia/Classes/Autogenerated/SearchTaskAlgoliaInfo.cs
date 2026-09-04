using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Algolia.Classes;
using ETG.Algolia.Providers;
using CMS.Search;

[assembly: RegisterObjectType(typeof(SearchTaskAlgoliaInfo), SearchTaskAlgoliaInfo.OBJECT_TYPE)]
    
namespace ETG.Algolia.Classes
{
    /// <summary>
    /// Data container class for <see cref="SearchTaskAlgoliaInfo"/>.
    /// </summary>
	[Serializable]
    public partial class SearchTaskAlgoliaInfo : AbstractInfo<SearchTaskAlgoliaInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.searchtaskalgolia";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(SearchTaskAlgoliaInfoProvider), OBJECT_TYPE, "ETG.SearchTaskAlgolia", "SearchTaskAlgoliaID", null, null, null, null, null, null, null, null)
        {
			ModuleName = "ETG.Algolia",
			TouchCacheDependencies = true,
        };


		/// <summary>
        /// Search task algolia ID.
        /// </summary>
		[DatabaseField]
        public virtual int SearchTaskAlgoliaID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SearchTaskAlgoliaID"), 0);
            }
            set
            {
                SetValue("SearchTaskAlgoliaID", value);
            }
        }


        [DatabaseField(ColumnName = "SearchTaskAlgoliaType", ValueType = typeof (string))]
        public virtual SearchTaskTypeEnum SearchTaskAlgoliaType
        {
            get => GetStringValue(nameof (SearchTaskAlgoliaType), "").ToEnum<SearchTaskTypeEnum>();
            set => SetValue(nameof (SearchTaskAlgoliaType), value.ToStringRepresentation());
        }


		/// <summary>
        /// Search task algolia object type.
        /// </summary>
		[DatabaseField]
        public virtual string SearchTaskAlgoliaObjectType
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SearchTaskAlgoliaObjectType"), String.Empty);
            }
            set
            {
                SetValue("SearchTaskAlgoliaObjectType", value, String.Empty);
            }
        }


		/// <summary>
        /// Search task algolia metadata.
        /// </summary>
		[DatabaseField]
        public virtual string SearchTaskAlgoliaMetadata
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SearchTaskAlgoliaMetadata"), String.Empty);
            }
            set
            {
                SetValue("SearchTaskAlgoliaMetadata", value, String.Empty);
            }
        }


		/// <summary>
        /// Search task algolia additional data.
        /// </summary>
		[DatabaseField]
        public virtual string SearchTaskAlgoliaAdditionalData
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SearchTaskAlgoliaAdditionalData"), String.Empty);
            }
            set
            {
                SetValue("SearchTaskAlgoliaAdditionalData", value);
            }
        }


		/// <summary>
        /// Search task algolia initiator object ID.
        /// </summary>
		[DatabaseField]
        public virtual int SearchTaskAlgoliaInitiatorObjectID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SearchTaskAlgoliaInitiatorObjectID"), 0);
            }
            set
            {
                SetValue("SearchTaskAlgoliaInitiatorObjectID", value, 0);
            }
        }


		/// <summary>
        /// Search task algolia priority.
        /// </summary>
		[DatabaseField]
        public virtual int SearchTaskAlgoliaPriority
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("SearchTaskAlgoliaPriority"), 0);
            }
            set
            {
                SetValue("SearchTaskAlgoliaPriority", value);
            }
        }


		/// <summary>
        /// Search task algolia error message.
        /// </summary>
		[DatabaseField]
        public virtual string SearchTaskAlgoliaErrorMessage
        {
            get
            {
                return ValidationHelper.GetString(GetValue("SearchTaskAlgoliaErrorMessage"), String.Empty);
            }
            set
            {
                SetValue("SearchTaskAlgoliaErrorMessage", value, String.Empty);
            }
        }


		/// <summary>
        /// Search task algolia created.
        /// </summary>
		[DatabaseField]
        public virtual DateTime SearchTaskAlgoliaCreated
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("SearchTaskAlgoliaCreated"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("SearchTaskAlgoliaCreated", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            SearchTaskAlgoliaInfoProvider.DeleteSearchTaskAlgoliaInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            SearchTaskAlgoliaInfoProvider.SetSearchTaskAlgoliaInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected SearchTaskAlgoliaInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="SearchTaskAlgoliaInfo"/> class.
        /// </summary>
        public SearchTaskAlgoliaInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="SearchTaskAlgoliaInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public SearchTaskAlgoliaInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}