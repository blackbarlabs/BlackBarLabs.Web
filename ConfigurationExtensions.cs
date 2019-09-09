﻿using EastFive.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EastFive.Web.Configuration
{
    public static class ConfigurationExtensions
    {
        public static TResult ConfigurationString<TResult>(this string key,
            Func<string, TResult> onFound,
            Func<string, TResult> onUnspecified = default)
        {
            return BlackBarLabs.Web.ConfigurationContext.Instance.GetSettingValue(key,
                onFound,
                () =>
                {
                    var msg = $" - The configuration value for [{key}] is not specified. Please specify a string value";
                    if (!onUnspecified.IsDefault())
                        return onUnspecified(msg);
                    throw new ConfigurationException(key, typeof(string), msg);
                });
        }

        private static TResult ConfigurationBase<TBase, TResult>(string key,
                Func<string, Func<string, TResult>, TResult> convert,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationString(key,
                keyValue =>
                {
                    return convert(keyValue,
                        (why) =>
                        {
                            var msg = $"The configuration value for [{key}] is invalid:{why}.\nPlease specify a valid {typeof(TBase).Name} value.";

                            if (!onFailure.IsDefault())
                                return onFailure(msg);

                            throw new ConfigurationException(key, typeof(TBase), msg);
                        });
                },
                (why) =>
                {
                    if (!onNotSpecified.IsDefault())
                        return onNotSpecified();

                    var msg = $" - The configuration value for [{key}] is not specified. Please specify a {typeof(TBase).Name} value.";
                    if (onFailure.IsDefault())
                        return onFailure(msg);

                    throw new ConfigurationException(key, typeof(TBase), msg);
                });
        }

        public static TResult ConfigurationBoolean<TResult>(this string key,
            Func<bool, TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<bool, TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (bool.TryParse(keyValue, out bool boolValue))
                        return onParsed(boolValue);

                    return failureCallback("Could not evaluate bool.");
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }

        public static TResult ConfigurationUri<TResult>(this string key,
            Func<Uri, TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<Uri, TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (Uri.TryCreate(keyValue, UriKind.RelativeOrAbsolute, out Uri uriValue))
                        return onParsed(uriValue);

                    return failureCallback("Could not evaluate URI.");
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }

        public static TResult ConfigurationDateTime<TResult>(this string key,
            Func<DateTime, TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<DateTime, TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (DateTime.TryParse(keyValue, out DateTime dateTimeValue))
                    {
                        var dateTimeValueUtc = dateTimeValue.ToUniversalTime();
                        return onParsed(dateTimeValueUtc);
                    }

                    return failureCallback("Could not evaluate DateTime.");
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }

        public static TResult ConfigurationDouble<TResult>(this string key,
            Func<double, TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<double, TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (double.TryParse(keyValue, out double doubleValue))
                        return onParsed(doubleValue);

                    return failureCallback("Could not evaluate double.");
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }

        public static TResult ConfigurationGuid<TResult>(this string key,
            Func<Guid, TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<Guid, TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (Guid.TryParse(keyValue, out Guid guidValue))
                        return onParsed(guidValue);

                    return failureCallback("Could not evaluate Guid.");
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }

        public static TResult ConfigurationBase64Bytes<TResult>(this string key,
            Func<byte[], TResult> onParsed,
            Func<string, TResult> onFailure = default,
            Func<TResult> onNotSpecified = default)
        {
            return ConfigurationBase<byte[], TResult>(key,
                (keyValue, failureCallback) =>
                {
                    if (keyValue == null)
                        return onNotSpecified();
                    try
                    {
                        var bytes = Convert.FromBase64String(keyValue);
                        return onParsed(bytes);
                    }
                    catch (FormatException ex)
                    {
                        return failureCallback(ex.Message);
                    }
                },
                onFailure: onFailure,
                onNotSpecified: onNotSpecified);
        }
    }
}
