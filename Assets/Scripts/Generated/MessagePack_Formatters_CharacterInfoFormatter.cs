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

namespace MessagePack.Formatters
{
    public sealed class CharacterInfoFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::CharacterInfo>
    {
        // characterName
        private static global::System.ReadOnlySpan<byte> GetSpan_characterName() => new byte[1 + 13] { 173, 99, 104, 97, 114, 97, 99, 116, 101, 114, 78, 97, 109, 101 };
        // defaultHealth
        private static global::System.ReadOnlySpan<byte> GetSpan_defaultHealth() => new byte[1 + 13] { 173, 100, 101, 102, 97, 117, 108, 116, 72, 101, 97, 108, 116, 104 };
        // defaultEnergy
        private static global::System.ReadOnlySpan<byte> GetSpan_defaultEnergy() => new byte[1 + 13] { 173, 100, 101, 102, 97, 117, 108, 116, 69, 110, 101, 114, 103, 121 };
        // defaultMoney
        private static global::System.ReadOnlySpan<byte> GetSpan_defaultMoney() => new byte[1 + 12] { 172, 100, 101, 102, 97, 117, 108, 116, 77, 111, 110, 101, 121 };
        // currentHealth
        private static global::System.ReadOnlySpan<byte> GetSpan_currentHealth() => new byte[1 + 13] { 173, 99, 117, 114, 114, 101, 110, 116, 72, 101, 97, 108, 116, 104 };
        // currentEnergy
        private static global::System.ReadOnlySpan<byte> GetSpan_currentEnergy() => new byte[1 + 13] { 173, 99, 117, 114, 114, 101, 110, 116, 69, 110, 101, 114, 103, 121 };
        // currentMoney
        private static global::System.ReadOnlySpan<byte> GetSpan_currentMoney() => new byte[1 + 12] { 172, 99, 117, 114, 114, 101, 110, 116, 77, 111, 110, 101, 121 };
        // items
        private static global::System.ReadOnlySpan<byte> GetSpan_items() => new byte[1 + 5] { 165, 105, 116, 101, 109, 115 };

        public void Serialize(ref global::MessagePack.MessagePackWriter writer, global::CharacterInfo value, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }

            var formatterResolver = options.Resolver;
            writer.WriteMapHeader(8);
            writer.WriteRaw(GetSpan_characterName());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Serialize(ref writer, value.characterName, options);
            writer.WriteRaw(GetSpan_defaultHealth());
            writer.Write(value.defaultHealth);
            writer.WriteRaw(GetSpan_defaultEnergy());
            writer.Write(value.defaultEnergy);
            writer.WriteRaw(GetSpan_defaultMoney());
            writer.Write(value.defaultMoney);
            writer.WriteRaw(GetSpan_currentHealth());
            writer.Write(value.currentHealth);
            writer.WriteRaw(GetSpan_currentEnergy());
            writer.Write(value.currentEnergy);
            writer.WriteRaw(GetSpan_currentMoney());
            writer.Write(value.currentMoney);
            writer.WriteRaw(GetSpan_items());
            global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::ItemInfo>>(formatterResolver).Serialize(ref writer, value.items, options);
        }

        public global::CharacterInfo Deserialize(ref global::MessagePack.MessagePackReader reader, global::MessagePack.MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }

            options.Security.DepthStep(ref reader);
            var formatterResolver = options.Resolver;
            var length = reader.ReadMapHeader();
            var ____result = new global::CharacterInfo();

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.Internal.CodeGenHelpers.ReadStringSpan(ref reader);
                switch (stringKey.Length)
                {
                    default:
                    FAIL:
                      reader.Skip();
                      continue;
                    case 13:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 7310577365311121507UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 435626790514UL) { goto FAIL; }

                                ____result.characterName = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<string>(formatterResolver).Deserialize(ref reader, options);
                                continue;

                            case 5220917119430321508UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 448629858661UL) { goto FAIL; }

                                ____result.defaultHealth = reader.ReadInt32();
                                continue;

                            case 5004744337316537700UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 521426593134UL) { goto FAIL; }

                                ____result.defaultEnergy = reader.ReadInt32();
                                continue;

                            case 5220919250020103523UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 448629858661UL) { goto FAIL; }

                                ____result.currentHealth = reader.ReadInt32();
                                continue;

                            case 5004746467906319715UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 521426593134UL) { goto FAIL; }

                                ____result.currentEnergy = reader.ReadInt32();
                                continue;

                        }
                    case 12:
                        switch (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey))
                        {
                            default: goto FAIL;
                            case 5581205089619961188UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 2036690543UL) { goto FAIL; }

                                ____result.defaultMoney = reader.ReadInt32();
                                continue;

                            case 5581207220209743203UL:
                                if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 2036690543UL) { goto FAIL; }

                                ____result.currentMoney = reader.ReadInt32();
                                continue;

                        }
                    case 5:
                        if (global::MessagePack.Internal.AutomataKeyGen.GetKey(ref stringKey) != 495756604521UL) { goto FAIL; }

                        ____result.items = global::MessagePack.FormatterResolverExtensions.GetFormatterWithVerify<global::System.Collections.Generic.List<global::ItemInfo>>(formatterResolver).Deserialize(ref reader, options);
                        continue;

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
