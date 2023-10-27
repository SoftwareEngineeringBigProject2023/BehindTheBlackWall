// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Resolvers
{
    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        private GeneratedResolver()
        {
        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        private static class FormatterCache<T>
        {
            internal static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> Formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    Formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        private static readonly global::System.Collections.Generic.Dictionary<global::System.Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<global::System.Type, int>(23)
            {
                { typeof(global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.NotifyMessage>>), 0 },
                { typeof(global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.SubmitMessage>>), 1 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>), 2 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.ComponentNotifyMessageDataPack>), 3 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.ComponentSubmitMessageDataPack>), 4 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.EId>), 5 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.Entity>), 6 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.NotifyMessage>), 7 },
                { typeof(global::System.Collections.Generic.List<global::SEServer.Data.SubmitMessage>), 8 },
                { typeof(global::SEServer.Data.AuthorizationMessage), 9 },
                { typeof(global::SEServer.Data.CId), 10 },
                { typeof(global::SEServer.Data.ComponentArrayDataPack), 11 },
                { typeof(global::SEServer.Data.ComponentNotifyMessageDataPack), 12 },
                { typeof(global::SEServer.Data.ComponentSubmitMessageDataPack), 13 },
                { typeof(global::SEServer.Data.EId), 14 },
                { typeof(global::SEServer.Data.Entity), 15 },
                { typeof(global::SEServer.Data.NotifyMessage), 16 },
                { typeof(global::SEServer.Data.Snapshot), 17 },
                { typeof(global::SEServer.Data.SnapshotMessage), 18 },
                { typeof(global::SEServer.Data.SubmitEntityMessage), 19 },
                { typeof(global::SEServer.Data.SubmitMessage), 20 },
                { typeof(global::SEServer.Data.SyncEntityMessage), 21 },
                { typeof(global::SEServer.Data.WId), 22 },
            };
        }

        internal static object GetFormatter(global::System.Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key))
            {
                return null;
            }

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.DictionaryFormatter<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.NotifyMessage>>();
                case 1: return new global::MessagePack.Formatters.DictionaryFormatter<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.SubmitMessage>>();
                case 2: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.ComponentArrayDataPack>();
                case 3: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.ComponentNotifyMessageDataPack>();
                case 4: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.ComponentSubmitMessageDataPack>();
                case 5: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.EId>();
                case 6: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.Entity>();
                case 7: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.NotifyMessage>();
                case 8: return new global::MessagePack.Formatters.ListFormatter<global::SEServer.Data.SubmitMessage>();
                case 9: return new MessagePack.Formatters.SEServer.Data.AuthorizationMessageFormatter();
                case 10: return new MessagePack.Formatters.SEServer.Data.CIdFormatter();
                case 11: return new MessagePack.Formatters.SEServer.Data.ComponentArrayDataPackFormatter();
                case 12: return new MessagePack.Formatters.SEServer.Data.ComponentNotifyMessageDataPackFormatter();
                case 13: return new MessagePack.Formatters.SEServer.Data.ComponentSubmitMessageDataPackFormatter();
                case 14: return new MessagePack.Formatters.SEServer.Data.EIdFormatter();
                case 15: return new MessagePack.Formatters.SEServer.Data.EntityFormatter();
                case 16: return new MessagePack.Formatters.SEServer.Data.NotifyMessageFormatter();
                case 17: return new MessagePack.Formatters.SEServer.Data.SnapshotFormatter();
                case 18: return new MessagePack.Formatters.SEServer.Data.SnapshotMessageFormatter();
                case 19: return new MessagePack.Formatters.SEServer.Data.SubmitEntityMessageFormatter();
                case 20: return new MessagePack.Formatters.SEServer.Data.SubmitMessageFormatter();
                case 21: return new MessagePack.Formatters.SEServer.Data.SyncEntityMessageFormatter();
                case 22: return new MessagePack.Formatters.SEServer.Data.WIdFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1649 // File name should match first type name




// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY MPC(MessagePack-CSharp). DO NOT CHANGE IT.
// </auto-generated>

#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168
#pragma warning disable CS1591 // document public APIs

#pragma warning disable SA1129 // Do not use default value type constructor
#pragma warning disable SA1309 // Field names should not begin with underscore
#pragma warning disable SA1312 // Variable names should begin with lower-case letter
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1649 // File name should match first type name

namespace MessagePack.Formatters.SEServer.Data
{
    public sealed class AuthorizationMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.AuthorizationMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.AuthorizationMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(1);
            writer.Write(value.UserId);
        }

        public global::SEServer.Data.AuthorizationMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.AuthorizationMessage();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.UserId = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class CIdFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.CId>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.CId value, global::MessagePack.MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(1);
            writer.Write(value.Id);
        }

        public global::SEServer.Data.CId Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new global::System.InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.CId();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.Id = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class ComponentArrayDataPackFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.ComponentArrayDataPack>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.ComponentArrayDataPack value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(2);
            writer.Write(value.TypeCode);
            writer.Write(value.Data);
        }

        public global::SEServer.Data.ComponentArrayDataPack Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.ComponentArrayDataPack();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.TypeCode = reader.ReadInt32();
                        break;
                    case 1:
                        ____result.Data = global::MessagePack.Internal.CodeGenHelpers.GetArrayFromNullableSequence(reader.ReadBytes());
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class ComponentNotifyMessageDataPackFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.ComponentNotifyMessageDataPack>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.ComponentNotifyMessageDataPack value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            writer.Write(value.TypeCode);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.NotifyMessage>>>(formatterResolver).Serialize(ref writer, value.NotifyMessages, options);
        }

        public global::SEServer.Data.ComponentNotifyMessageDataPack Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.ComponentNotifyMessageDataPack();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.TypeCode = reader.ReadInt32();
                        break;
                    case 1:
                        ____result.NotifyMessages = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.NotifyMessage>>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class ComponentSubmitMessageDataPackFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.ComponentSubmitMessageDataPack>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.ComponentSubmitMessageDataPack value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            writer.Write(value.TypeCode);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.SubmitMessage>>>(formatterResolver).Serialize(ref writer, value.SubmitMessages, options);
        }

        public global::SEServer.Data.ComponentSubmitMessageDataPack Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.ComponentSubmitMessageDataPack();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.TypeCode = reader.ReadInt32();
                        break;
                    case 1:
                        ____result.SubmitMessages = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.Dictionary<global::SEServer.Data.CId, global::System.Collections.Generic.List<global::SEServer.Data.SubmitMessage>>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class EIdFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.EId>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.EId value, global::MessagePack.MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(1);
            writer.Write(value.Id);
        }

        public global::SEServer.Data.EId Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new global::System.InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.EId();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.Id = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class EntityFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.Entity>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.Entity value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(1);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.EId>(formatterResolver).Serialize(ref writer, value.Id, options);
        }

        public global::SEServer.Data.Entity Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.Entity();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.Id = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.EId>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class NotifyMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.NotifyMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.NotifyMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(0);
        }

        public global::SEServer.Data.NotifyMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            reader.Skip();
            return new global::SEServer.Data.NotifyMessage();
        }
    }

    public sealed class SnapshotFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.Snapshot>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.Snapshot value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(3);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.WId>(formatterResolver).Serialize(ref writer, value.WorldId, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.Entity>>(formatterResolver).Serialize(ref writer, value.Entities, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Serialize(ref writer, value.ComponentArrayDataPacks, options);
        }

        public global::SEServer.Data.Snapshot Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.Snapshot();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.WorldId = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.WId>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 1:
                        ____result.Entities = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.Entity>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 2:
                        ____result.ComponentArrayDataPacks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class SnapshotMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.SnapshotMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.SnapshotMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(1);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.Snapshot>(formatterResolver).Serialize(ref writer, value.Snapshot, options);
        }

        public global::SEServer.Data.SnapshotMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.SnapshotMessage();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.Snapshot = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::SEServer.Data.Snapshot>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class SubmitEntityMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.SubmitEntityMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.SubmitEntityMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(2);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Serialize(ref writer, value.ComponentArrayDataPacks, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentSubmitMessageDataPack>>(formatterResolver).Serialize(ref writer, value.ComponentSubmitDataPacks, options);
        }

        public global::SEServer.Data.SubmitEntityMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.SubmitEntityMessage();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.ComponentArrayDataPacks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 1:
                        ____result.ComponentSubmitDataPacks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentSubmitMessageDataPack>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class SubmitMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.SubmitMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.SubmitMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            writer.WriteArrayHeader(0);
        }

        public global::SEServer.Data.SubmitMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            reader.Skip();
            return new global::SEServer.Data.SubmitMessage();
        }
    }

    public sealed class SyncEntityMessageFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.SyncEntityMessage>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.SyncEntityMessage value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }

            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            writer.WriteArrayHeader(4);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.EId>>(formatterResolver).Serialize(ref writer, value.EntitiesToDelete, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.EId>>(formatterResolver).Serialize(ref writer, value.EntitiesToCreate, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Serialize(ref writer, value.ComponentArrayDataPacks, options);
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentNotifyMessageDataPack>>(formatterResolver).Serialize(ref writer, value.ComponentNotifyDataPacks, options);
        }

        public global::SEServer.Data.SyncEntityMessage Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            global::MessagePack.IFormatterResolver formatterResolver = options.Resolver;
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.SyncEntityMessage();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.EntitiesToDelete = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.EId>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 1:
                        ____result.EntitiesToCreate = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.EId>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 2:
                        ____result.ComponentArrayDataPacks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentArrayDataPack>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    case 3:
                        ____result.ComponentNotifyDataPacks = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::SEServer.Data.ComponentNotifyMessageDataPack>>(formatterResolver).Deserialize(ref reader, options);
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

    public sealed class WIdFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::SEServer.Data.WId>
    {

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::SEServer.Data.WId value, global::MessagePack.MessagePackSerializerOptions options)
        {
            writer.WriteArrayHeader(1);
            writer.Write(value.Id);
        }

        public global::SEServer.Data.WId Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                throw new global::System.InvalidOperationException("typecode is null, struct not supported");
            }

            options.Security.DepthStep(ref reader);
            var length = reader.ReadArrayHeader();
            var ____result = new global::SEServer.Data.WId();

            for (int i = 0; i < length; i++)
            {
                switch (i)
                {
                    case 0:
                        ____result.Id = reader.ReadInt32();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            reader.Depth--;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612

#pragma warning restore SA1129 // Do not use default value type constructor
#pragma warning restore SA1309 // Field names should not begin with underscore
#pragma warning restore SA1312 // Variable names should begin with lower-case letter
#pragma warning restore SA1403 // File may only contain a single namespace
#pragma warning restore SA1649 // File name should match first type name

