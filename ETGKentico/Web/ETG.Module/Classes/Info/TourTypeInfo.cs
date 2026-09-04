using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using ETG.Module.Classes.Info;
using ETG.Module.Classes.Providers;

[assembly: RegisterObjectType(typeof(TourTypeInfo), TourTypeInfo.OBJECT_TYPE)]

namespace ETG.Module.Classes.Info
{
    /// <summary>
    /// Data container class for <see cref="TourTypeInfo"/>.
    /// </summary>
    [Serializable]
    public partial class TourTypeInfo : AbstractInfo<TourTypeInfo>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "etg.tourtype";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(TourTypeInfoProvider), OBJECT_TYPE, "ETG.TourType", "TourTypeID", "TourTypeLastModified", "TourTypeGuid", "TourTypeCodeName", "TourTypeName", null, null, null, null)
        {
            ModuleName = "ETG",
            TouchCacheDependencies = true,
        };


        /// <summary>
        /// Tour type ID.
        /// </summary>
        [DatabaseField]
        public virtual int TourTypeID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("TourTypeID"), 0);
            }
            set
            {
                SetValue("TourTypeID", value);
            }
        }


        /// <summary>
        /// Tour type name.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeName"), String.Empty);
            }
            set
            {
                SetValue("TourTypeName", value);
            }
        }


        /// <summary>
        /// Tour type code name.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeCodeName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeCodeName"), String.Empty);
            }
            set
            {
                SetValue("TourTypeCodeName", value);
            }
        }


        /// <summary>
        /// Tour type icon.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeIcon
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeIcon"), String.Empty);
            }
            set
            {
                SetValue("TourTypeIcon", value);
            }
        }


        /// <summary>
        /// Tour type white icon image.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeWhiteIconImage
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeWhiteIconImage"), String.Empty);
            }
            set
            {
                SetValue("TourTypeWhiteIconImage", value, String.Empty);
            }
        }


        /// <summary>
        /// Tour type black icon image.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeBlackIconImage
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeBlackIconImage"), String.Empty);
            }
            set
            {
                SetValue("TourTypeBlackIconImage", value, String.Empty);
            }
        }


        /// <summary>
        /// Tour type description.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeDescription
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeDescription"), String.Empty);
            }
            set
            {
                SetValue("TourTypeDescription", value, String.Empty);
            }
        }


        /// <summary>
        /// Tour type url.
        /// </summary>
        [DatabaseField]
        public virtual string TourTypeUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("TourTypeUrl"), String.Empty);
            }
            set
            {
                SetValue("TourTypeUrl", value, String.Empty);
            }
        }


        /// <summary>
        /// Tour type guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid TourTypeGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("TourTypeGuid"), Guid.Empty);
            }
            set
            {
                SetValue("TourTypeGuid", value);
            }
        }


        /// <summary>
        /// Tour type last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime TourTypeLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("TourTypeLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("TourTypeLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            TourTypeInfoProvider.DeleteTourTypeInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            TourTypeInfoProvider.SetTourTypeInfo(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected TourTypeInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="TourTypeInfo"/> class.
        /// </summary>
        public TourTypeInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="TourTypeInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public TourTypeInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}