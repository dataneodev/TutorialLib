﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dataneo.TutorialLibs.FileIO.Win.Translation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("dataneo.TutorialLibs.FileIO.Win.Translation.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Canceled at the request of the user.
        /// </summary>
        internal static string CANCELED_BY_USER {
            get {
                return ResourceManager.GetString("CANCELED_BY_USER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Directory not found.
        /// </summary>
        internal static string DIRECTORY_NOT_FOUND {
            get {
                return ResourceManager.GetString("DIRECTORY_NOT_FOUND", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error reading file length.
        /// </summary>
        internal static string ERROR_READING_FILE_LENGTH {
            get {
                return ResourceManager.GetString("ERROR_READING_FILE_LENGTH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error reading video length. File {0}.
        /// </summary>
        internal static string ERROR_READING_VIDEO_DURATION {
            get {
                return ResourceManager.GetString("ERROR_READING_VIDEO_DURATION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error searching for files in a folder. File: {0}.
        /// </summary>
        internal static string ERROR_SEARCHING_FILES_IN_FOLDER {
            get {
                return ResourceManager.GetString("ERROR_SEARCHING_FILES_IN_FOLDER", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to File not found.
        /// </summary>
        internal static string FILE_NOT_FOUND {
            get {
                return ResourceManager.GetString("FILE_NOT_FOUND", resourceCulture);
            }
        }
    }
}