//
// Copyright 2014 Carbonfrost Systems, Inc. (http://carbonfrost.com)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Carbonfrost.Commons.Core.Runtime;

namespace Carbonfrost.Commons.Core.Runtime {

    class PropertiesObjectMetaObject : DynamicMetaObject {

        internal PropertiesObjectMetaObject(Expression parameter, PropertiesObject value)
            : base(parameter, BindingRestrictions.Empty, value) {}

        private static readonly MethodInfo GetPropertyMethod
            = typeof(IPropertyProvider).Assembly
                .GetType("Carbonfrost.Commons.Core.Runtime.Extensions")
                .GetMethods()
                .Single(t => !t.IsGenericMethod && t.Name == "GetProperty");

        private static readonly MethodInfo SetPropertyMethod
            = typeof(IProperties).GetMethod("SetProperty");

        private BindingRestrictions DefaultRestrictions {
            get {
                return BindingRestrictions.GetTypeRestriction(Expression, LimitType);
            }
        }

        private PropertiesObject PropertiesObject {
            get {
                return (PropertiesObject) Value;
            }
        }

        public override DynamicMetaObject BindGetMember(GetMemberBinder binder) {
            var item = PropertiesObject.Properties;
            var expr = Expression.Call(GetPropertyMethod, Expression.Constant(item), Expression.Constant(binder.Name));
            return new DynamicMetaObject(expr, DefaultRestrictions);
        }

        public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes) {
            throw new NotImplementedException();
        }

        public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value) {
            var item = PropertiesObject.Properties;

            var expr = Expression.Block(Expression.Call(Expression.Constant(item), SetPropertyMethod, Expression.Constant(binder.Name), value.Expression),
                                        Expression.Constant(item));

            return new DynamicMetaObject(expr, DefaultRestrictions);
        }

        public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value) {
            throw new NotImplementedException();
        }
    }
}

