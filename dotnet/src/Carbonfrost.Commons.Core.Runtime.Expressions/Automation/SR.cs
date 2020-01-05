
// This file was automatically generated.  DO NOT EDIT or else
// your changes could be lost!

#pragma warning disable 1570

using System;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions.Resources {

    /// <summary>
    /// Contains strongly-typed string resources.
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("srgen", "1.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    internal static partial class SR {

        private static global::System.Resources.ResourceManager _resources;
        private static global::System.Globalization.CultureInfo _currentCulture;
        private static global::System.Func<string, string> _resourceFinder;

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resources, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Carbonfrost.Commons.Core.Runtime.Expressions.Automation.SR", typeof(SR).GetTypeInfo().Assembly);
                    _resources = temp;
                }
                return _resources;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _currentCulture;
            }
            set {
                _currentCulture = value;
            }
        }

        private static global::System.Func<string, string> ResourceFinder {
            get {
                if (object.ReferenceEquals(_resourceFinder, null)) {
                    try {
                        global::System.Resources.ResourceManager rm = ResourceManager;
                        _resourceFinder = delegate (string s) {
                            return rm.GetString(s);
                        };
                    } catch (global::System.Exception ex) {
                        _resourceFinder = delegate (string s) {
                            return string.Format("localization error! {0}: {1} ({2})", s, ex.GetType(), ex.Message);
                        };
                    }
                }
                return _resourceFinder;
            }
        }


  /// <summary>Cannot convert `undefined' to type `${type}'.</summary>
    internal static string CannotCastUndefined(
    object @type
    ) {
        return string.Format(Culture, ResourceFinder("CannotCastUndefined") , @type);
    }

  /// <summary>Expression is either unbound or non-constant and cannot be evaluated without an evaluation context.</summary>
    internal static string CannotEvaluateNonConstantExpression(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotEvaluateNonConstantExpression") );
    }

  /// <summary>Cannot reduce the current expression any further.</summary>
    internal static string CannotReduceExpression(
    
    ) {
        return string.Format(Culture, ResourceFinder("CannotReduceExpression") );
    }

  /// <summary>Index must specify an array of rank one.</summary>
    internal static string ExpressionContextInvalidIndexCount(
    
    ) {
        return string.Format(Culture, ResourceFinder("ExpressionContextInvalidIndexCount") );
    }

  /// <summary>Expression type is not a binary expression type: ${value}.</summary>
    internal static string NotBinaryExpressionType(
    object @value
    ) {
        return string.Format(Culture, ResourceFinder("NotBinaryExpressionType") , @value);
    }

  /// <summary>Reference error: `${name}' is not defined.</summary>
    internal static string ReferenceError(
    object @name
    ) {
        return string.Format(Culture, ResourceFinder("ReferenceError") , @name);
    }

  /// <summary>A session must be open for this operation.</summary>
    internal static string RequireSerializationManagerSession(
    
    ) {
        return string.Format(Culture, ResourceFinder("RequireSerializationManagerSession") );
    }

  /// <summary>Specified field or property must be public and static.</summary>
    internal static string RequiresStaticFieldOrProperty(
    
    ) {
        return string.Format(Culture, ResourceFinder("RequiresStaticFieldOrProperty") );
    }

    }
}
