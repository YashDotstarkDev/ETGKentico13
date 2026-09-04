using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

[assembly: RegisterObjectType(typeof(ArticleCategoryInfo), ArticleCategoryInfo.OBJECT_TYPE)]

namespace ETG.Module.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="ArticleCategoryInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ArticleCategoryInfo : AbstractInfo<ArticleCategoryInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.articlecategory";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ArticleCategoryInfoProvider), OBJECT_TYPE, "ETG.ArticleCategory", "ArticleCategoryID", "ArticleCategoryLastModified", "ArticleCategoryGuid", "ArticleCategoryCodeName", "ArticleCategoryName", null, null, null, null)
        {
            ModuleName = "ETG",
            TouchCacheDependencies = true,
            ImportExportSettings =
            {
                IsExportable = true, // Makes the data of the custom Office class exportable
                AllowSingleExport = true, // Allows export of single office objects from the office listing page
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the global objects export interface
                    new ObjectTreeLocation(GLOBAL, "ArticleCategoryCodeName")
                }
            },
            SynchronizationSettings =
            {
                LogSynchronization = SynchronizationTypeEnum.LogSynchronization, // Enables logging of staging tasks for changes made to Office objects
                ObjectTreeLocations = new List<ObjectTreeLocation>()
                {
                    // Creates a new category in the 'Global objects' section of the staging object tree
                    new ObjectTreeLocation(GLOBAL, "ArticleCategoryCodeName")
                }
            }
        };


        /// <summary>
        /// Article category ID.
        /// </summary>
        [DatabaseField]
        public virtual int ArticleCategoryID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ArticleCategoryID"), 0);
            }
            set
            {
                SetValue("ArticleCategoryID", value);
            }
        }


        /// <summary>
        /// Article category name.
        /// </summary>
        [DatabaseField]
        public virtual string ArticleCategoryName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ArticleCategoryName"), String.Empty);
            }
            set
            {
                SetValue("ArticleCategoryName", value);
            }
        }


        /// <summary>
        /// Article category code name.
        /// </summary>
        [DatabaseField]
        public virtual string ArticleCategoryCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ArticleCategoryCodeName"), String.Empty);
            }
            set
            {
                SetValue("ArticleCategoryCodeName", value);
            }
        }


        /// <summary>
        /// Article category guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid ArticleCategoryGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ArticleCategoryGuid"), Guid.Empty);
            }
            set
            {
                SetValue("ArticleCategoryGuid", value);
            }
        }


        /// <summary>
        /// Article category last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ArticleCategoryLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("ArticleCategoryLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("ArticleCategoryLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            ArticleCategoryInfoProvider.DeleteArticleCategoryInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            ArticleCategoryInfoProvider.SetArticleCategoryInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ArticleCategoryInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ArticleCategoryInfo"/> class.
        /// </summary>
        public ArticleCategoryInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ArticleCategoryInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ArticleCategoryInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}