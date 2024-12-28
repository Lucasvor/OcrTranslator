using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrTranslator.Helpers.Attributes;

/// <summary>
/// Adding this attribute to a property makes it not configurable from the command line.
/// Typical use cases:
/// - Property represents internal module state.
/// - Property has a type that is unwieldy to type as a command line string.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class CmdConfigureIgnoreAttribute : Attribute;
