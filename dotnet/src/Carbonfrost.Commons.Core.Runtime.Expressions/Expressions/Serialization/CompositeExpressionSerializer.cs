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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Carbonfrost.Commons.Core.Runtime.Expressions.Serialization {

    class CompositeExpressionSerializer : ExpressionSerializer {

        public override Expression ConvertToExpression(object item, IExpressionSerializerContext context) {
            if (ReferenceEquals(item, null))
                return Expression.Null;

            bool first = context.GetVariable(item) == null;
            var selfVarName = context.DefineVariable(item);

            if (first) {
                IList<Expression> results = new List<Expression>();

                // variable = new NewInstanceExpression();
                var expr1 = CreateInstance(selfVarName, item.GetType());
                StoreSerializationInfo(expr1, selfVarName);
                results.Add(expr1);

                // variable.Property = property1;
                // TODO Sharing this expression in the tree will cause errors in the future
                var self = Expression.Name(selfVarName);
                EvalProperties(item, context, self, results);
                EvalContent(item, context, self, results);

                var block = Expression.Block(results);
                StoreSerializationInfo(block, selfVarName);
                return block;

            } else {
                return Expression.Name(selfVarName);
            }
        }

        protected virtual void EvalContent(object item,
                                           IExpressionSerializerContext context,
                                           Expression qual,
                                           IList<Expression> results)
        {
            var content = TryAggregate(context, qual, item);
            if (content != null) {
                results.Add(content);
            }
        }

        private void EvalProperties(object item,
                                    IExpressionSerializerContext context,
                                    Expression qual,
                                    ICollection<Expression> results)
        {
            // TODO Use a stack to track circular references
            var properties = PropertyCache.ReflectGetPropertiesCache(item.GetType()).Values;

            foreach (PropertyInfo pd in properties) {
                var mode = pd.GetExpressionSerializationMode();
                if (mode == ExpressionSerializationMode.Hidden) {
                    continue;
                }

                object propertyValue;
                try {
                    propertyValue = pd.GetValue(item);

                } catch (Exception) {
                    // TODO Handling of exceptions should be more robust and allow ExceptionHandler
                    continue;
                }

                var propExpr = Expression.MemberAccess(qual, pd.Name);

                if (pd.SetMethod == null || !pd.SetMethod.IsPublic) {
                    if (mode == ExpressionSerializationMode.Content) {
                        var expr2 = TryAggregate(context, propExpr, propertyValue);
                        if (expr2 != null) {
                            results.Add(expr2);
                        }
                    }

                } else {
                    Expression pes = ExpressionSerializer.SerializeOrReference(propertyValue, context);
                    var expr = CreateProperty(propExpr, pes);
                    results.Add(expr);
                }
            }
        }

        private BlockExpression TryAggregate(IExpressionSerializerContext context,
                                             Expression qual,
                                             object propertyValue) {

            var items = propertyValue as System.Collections.IEnumerable;
            if (items == null) {
                return null;
            }

            var results = new List<Expression>();

            // TODO Estimate size of results list (performance)
            foreach (object component in items) {
                Expression pes = ExpressionSerializer.SerializeOrReference(component, context);

                var qualAdd = Expression.MemberAccess(qual, "Add");

                if (pes.ExpressionType != ExpressionType.Name) {
                    results.Add(pes);
                }

                results.Add(Expression.Call(qualAdd, new [] { Expression.Name(FindName(pes)) }));
            }

            if (results.Count == 0)
                return null;

            return Expression.Block(results);
        }

        private static Expression CreateProperty(Expression propertyFunc, Expression value) {
            return Expression.Assign(propertyFunc, value);
        }

        private static Expression CreateInstance(string name, Type type) {
            return Expression.Assign(Expression.Name(name), Expression.NewObject(CreateTypeReference(type)));
        }

        private static string FindName(Expression pes) {
            if (pes.ExpressionType == ExpressionType.Name) {
                return ((NameExpression) pes).Name;
            }

            var ann = pes.Annotation<SerializerInfo>();
            if (ann == null) {
                return "unknownVar";
            }

            return ann.Name;
        }
    }
}
