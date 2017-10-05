using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Grouping
{
    public class ObservableOrderedCollection<T> : ObservableCollection<T>
    {
        public ObservableOrderedCollection()
        {

        }

        //public ObservableOrderedCollection(IEnumerable<T> collection, Expression<Func<T, TOrderType>> orderBy) : base(collection.OrderBy(orderBy.Compile()))
        //{

        //}

        public ObservableOrderedCollection(IEnumerable<T> collection, Expression<Func<T>> orderBy) : base(OrderBy(collection, orderBy))
        {

        }

        public ObservableOrderedCollection(List<T> list) : base(list)
        {

        }

        static IEnumerable<T> OrderBy(IEnumerable<T> collection, Expression<Func<T>> orderBy)
        {
            var propertyName = orderBy.GetPropertyNameFromExpression(orderBy);

            // TODO

            return collection;
        }

        //static Func<T, TOrderType> GetOrderByFuncFromPropertyName(string propertyName)
        //{
        //    var propertyInfo = typeof(T).GetProperty(propertyName);
        //    if (propertyInfo == null)
        //        return new Func<T, TOrderType>(_ => default(TOrderType));
        //    return new Func<T, TOrderType>(item => (TOrderType)propertyInfo.GetValue(item));
        //}
    }

    public static class PropertyNameExtensionMethods
    {
        private const string WrongExpressionMessage = "Wrong expression\nshould be called with expression like\n() => PropertyName";
        private const string WrongUnaryExpressionMessage = "Wrong unary expression\nshould be called with expression like\n() => PropertyName";

        public static string GetPropertyNameFromExpression<T>(this object target, Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }
            var memberExpression = FindMemberExpression(expression);
            if (memberExpression == null)
            {
                throw new ArgumentException(WrongExpressionMessage, nameof(expression));
            }
            var member = memberExpression.Member as PropertyInfo;
            if (member == null)
            {
                throw new ArgumentException(WrongExpressionMessage, nameof(expression));
            }
            if (member.DeclaringType == null)
            {
                throw new ArgumentException(WrongExpressionMessage, nameof(expression));
            }
            if (target != null && !member.DeclaringType.IsInstanceOfType(target))
            {
                throw new ArgumentException(WrongExpressionMessage, nameof(expression));
            }
            if (member.GetGetMethod(true).IsStatic)
            {
                throw new ArgumentException(WrongExpressionMessage, nameof(expression));
            }
            return member.Name;
        }

        private static MemberExpression FindMemberExpression<T>(Expression<Func<T>> expression)
        {
            var body = expression.Body as UnaryExpression;
            if (body != null)
            {
                var unary = body;
                var member = unary.Operand as MemberExpression;
                if (member == null)
                    throw new ArgumentException(WrongUnaryExpressionMessage, nameof(expression));
                return member;
            }
            return expression.Body as MemberExpression;
        }
    }
}
