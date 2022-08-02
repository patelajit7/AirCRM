using Infrastructure.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DatabaseService : IDatabase
    {
        public static string ConnectionString = string.Empty;
        public DatabaseService(string connection)
        {
            ConnectionString = connection;
        }

        /// <summary>
        /// Insert row in entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="entity">entity</param>
        /// <returns>int</returns>
        public int Create<T>(T entity) where T : ContentBase
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    TypeSwitch.Do(
                    entity,
                    TypeSwitch.Case<Airports>(x => context.Airports.Attach(x)),
                    TypeSwitch.Case<Airlines>(x => context.Airlines.Attach(x)),
                    TypeSwitch.Case<Bookings>(x => context.Bookings.Attach(x)),
                    TypeSwitch.Case<FlightPriceDetails>(x => context.FlightPriceDetails.Attach(x)),
                    TypeSwitch.Case<FlightSegments>(x => context.FlightSegments.Attach(x)),
                    TypeSwitch.Case<Travellers>(x => context.Travellers.Attach(x)),
                    TypeSwitch.Case<FlightBookingRemarks>(x => context.FlightBookingRemarks.Attach(x)),
                    TypeSwitch.Case<Markups>(x => context.Markups.Attach(x)),
                    TypeSwitch.Case<Coupons>(x => context.Coupons.Attach(x)),
                    TypeSwitch.Default(() => { throw new Exception(string.Format("Type {0} not mapped in DatabaseService.Save<T>.", entity.GetType().ToString())); })
                    );
                    context.Entry<T>(entity).State = System.Data.Entity.EntityState.Added;
                    context.SaveChanges();
                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int Delete<T>(T entity) where T : ContentBase
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    TypeSwitch.Do(
                    entity,
                    TypeSwitch.Case<Airports>(x => context.Airports.Attach(x)),
                    TypeSwitch.Case<Airlines>(x => context.Airlines.Attach(x)),
                    TypeSwitch.Case<Bookings>(x => context.Bookings.Attach(x)),
                    TypeSwitch.Case<FlightPriceDetails>(x => context.FlightPriceDetails.Attach(x)),
                    TypeSwitch.Case<FlightSegments>(x => context.FlightSegments.Attach(x)),
                    TypeSwitch.Case<Travellers>(x => context.Travellers.Attach(x)),
                    TypeSwitch.Case<FlightBookingRemarks>(x => context.FlightBookingRemarks.Attach(x)),
                    TypeSwitch.Case<Markups>(x => context.Markups.Attach(x)),
                    TypeSwitch.Case<Coupons>(x => context.Coupons.Attach(x)),
                    TypeSwitch.Default(() => { throw new Exception(string.Format("Type {0} not mapped in DatabaseService.Save<T>.", entity.GetType().ToString())); })
                    );
                    context.Entry<T>(entity).State = System.Data.Entity.EntityState.Deleted;
                    context.SaveChanges();
                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update entity
        /// </summary>
        /// <typeparam name="T">entity</typeparam>
        /// <param name="entity">entity</param>
        public int Update<T>(T entity) where T : ContentBase
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    TypeSwitch.Do(
                    entity,
                    TypeSwitch.Case<Airports>(x => context.Airports.Attach(x)),
                    TypeSwitch.Case<Airlines>(x => context.Airlines.Attach(x)),
                    TypeSwitch.Case<Bookings>(x => context.Bookings.Attach(x)),
                    TypeSwitch.Case<FlightPriceDetails>(x => context.FlightPriceDetails.Attach(x)),
                    TypeSwitch.Case<FlightSegments>(x => context.FlightSegments.Attach(x)),
                    TypeSwitch.Case<Travellers>(x => context.Travellers.Attach(x)),
                    TypeSwitch.Case<FlightBookingRemarks>(x => context.FlightBookingRemarks.Attach(x)),
                    TypeSwitch.Case<Markups>(x => context.Markups.Attach(x)),
                    TypeSwitch.Case<Coupons>(x => context.Coupons.Attach(x)),
                    TypeSwitch.Default(() => { throw new Exception(string.Format("Type {0} not mapped in DatabaseService.Save<T>.", entity.GetType().ToString())); })
                    );
                    context.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> List<T>() where T : ContentBase
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    List<T> entityList = null;
                    TypeSwitch.Do<T>(
                    null,
                    TypeSwitch.Case<Airports>(() => { entityList = context.Airports.ToList() as List<T>; }),
                    TypeSwitch.Case<Airlines>(() => { entityList = context.Airlines.ToList() as List<T>; }),
                    TypeSwitch.Case<Bookings>(() => { entityList = context.Bookings.ToList() as List<T>; }),
                    TypeSwitch.Case<FlightPriceDetails>(() => { entityList = context.FlightPriceDetails.ToList() as List<T>; }),
                    TypeSwitch.Case<FlightSegments>(() => { entityList = context.FlightSegments.ToList() as List<T>; }),
                    TypeSwitch.Case<Travellers>(() => { entityList = context.Travellers.ToList() as List<T>; }),
                    TypeSwitch.Case<FlightBookingRemarks>(() => { entityList = context.FlightBookingRemarks.ToList() as List<T>; }),
                    TypeSwitch.Case<Markups>(() => { entityList = context.Markups.ToList() as List<T>; }),
                    TypeSwitch.Case<Coupons>(() => { entityList = context.Coupons.ToList() as List<T>; }),
                    TypeSwitch.Default(() => { throw new Exception(string.Format("Type {0} not mapped in DatabaseService.List<T>.", typeof(T).ToString())); })
                    );
                    return entityList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<T> Where<T>(Expression<Func<T, bool>> expression) where T : ContentBase
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    IQueryable<T> entity = null;
                    TypeSwitch.Do<T>(
                    expression,
                    TypeSwitch.Case<Airports, Expression<Func<Airports, bool>>>(ex => { entity = context.Airports.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<Airlines, Expression<Func<Airlines, bool>>>(ex => { entity = context.Airlines.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<Bookings, Expression<Func<Bookings, bool>>>(ex => { entity = context.Bookings.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<FlightPriceDetails, Expression<Func<FlightPriceDetails, bool>>>(ex => { entity = context.FlightPriceDetails.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<FlightSegments, Expression<Func<FlightSegments, bool>>>(ex => { entity = context.FlightSegments.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<Travellers, Expression<Func<Travellers, bool>>>(ex => { entity = context.Travellers.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<FlightBookingRemarks, Expression<Func<FlightBookingRemarks, bool>>>(ex => { entity = context.FlightBookingRemarks.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<Markups, Expression<Func<Markups, bool>>>(ex => { entity = context.Markups.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Case<Coupons, Expression<Func<Coupons, bool>>>(ex => { entity = context.Coupons.Where(ex) as IQueryable<T>; }),
                    TypeSwitch.Default(() => { throw new Exception(string.Format("Type {0} not mapped in DatabaseService.Where<T>.", typeof(T).ToString())); })
                    );
                    return entity.ToList<T>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<T> ExecuteQuery<T>(string query, List<SqlParameter> param)
        {
            try
            {
                using (ConnectionContext context = new ConnectionContext())
                {
                    List<T> objResult = null;
                    var result = context.Database.SqlQuery<T>(query, param == null ? new List<SqlParameter>().ToArray() : param.ToArray());
                    objResult = result.ToList<T>();
                    return objResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class ConnectionContext : DbContext
    {
        public ConnectionContext()
            : base(DatabaseService.ConnectionString)
        {
            var objectContext = (this as IObjectContextAdapter).ObjectContext;
            objectContext.CommandTimeout = 3600;
        }

        public DbSet<Airports> Airports { get; set; }
        public DbSet<Airlines> Airlines { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<FlightPriceDetails> FlightPriceDetails { get; set; }
        public DbSet<FlightSegments> FlightSegments { get; set; }
        public DbSet<Travellers> Travellers { get; set; }
        public DbSet<FlightBookingRemarks> FlightBookingRemarks { get; set; }
        public DbSet<Markups> Markups { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
    }
    public static class TypeSwitch
    {
        public class CaseInfo
        {
            public bool IsDefault { get; set; }
            public Type Target { get; set; }
            public Action<object> Action { get; set; }
        }

        public static void Do(object source, params CaseInfo[] cases)
        {
            var type = source.GetType();
            foreach (var entry in cases)
            {
                if (entry.IsDefault || type == entry.Target)
                {
                    entry.Action(source);
                    break;
                }
            }
        }

        public static void Do<T>(object source, params CaseInfo[] cases)
        {
            foreach (var entry in cases)
            {
                if (entry.IsDefault || typeof(T) == entry.Target)
                {
                    entry.Action(source);
                    break;
                }
            }
        }



        public static CaseInfo Case<T>(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                Target = typeof(T)
            };
        }

        public static CaseInfo Case<T>(Action<T> action)
        {
            return new CaseInfo()
            {
                Action = (x) => action((T)x),
                Target = typeof(T)
            };
        }

        public static CaseInfo Case<T, K>(Action<K> action)
        {
            return new CaseInfo()
            {
                Action = (x) => action((K)x),
                Target = typeof(T)
            };
        }


        public static CaseInfo Default(Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                IsDefault = true
            };
        }
    }
}


