using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TerminalCommand
{
    private string _commandName;
    private string _commandDescription;
    private string _commandSyntax;

    public string commandName { get { return _commandName; } }
    public string commandDescription { get { return _commandDescription;  } }
    public string commandSyntax { get { return _commandSyntax; } }

    public TerminalCommand(string name, string description, string syntax)
    {
        _commandName = name;
        _commandDescription = description;
        _commandSyntax = syntax;
    }

}

public class Command : TerminalCommand
{
    private Action command;
    public Command(string name, string description, string syntax, Action command) : base (name, description, syntax)
    {
        this.command = command;
    }

    public void Invoke()
    {
        command.Invoke();
    }
}

public class Command<T1> : TerminalCommand //uses a tuple to specifiy an input parameter
{
    private Action<T1> command;

    public Command(string name, string description, string syntax, Action<T1> command) : base(name, description, syntax)
    {
        this.command = command;
    }

    public void Invoke(T1 value)
    {
        command.Invoke(value);
    }
}
