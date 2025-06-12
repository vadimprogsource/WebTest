﻿using System;
namespace Test.Api.Domain;

public interface IForkFaultFactory
{
    Task<IForkFault> CreateInstanceAsync(int forkId);
}

