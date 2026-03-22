namespace Zuhid.Product.Enums;

[Flags]
public enum EnumFlagType : uint {

  None = 0, // always include 0
  Read = 1 << 0, // 1
  Write = 1 << 1, // 2
  Execute = 1 << 2, // 4
  Delete = 1 << 3, // 8
  Share = 1 << 4, // 16
  Admin = 1 << 5, // 32

  // Optional convenience composites:
  ReadWrite = Read | Write,
  AllBasic = Read | Write | Execute,
  All = Read | Write | Execute | Delete | Share | Admin
}
