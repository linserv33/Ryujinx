﻿namespace Ryujinx.Graphics.GAL.Multithreading.Commands
{
    struct SetFaceCullingCommand : IGALCommand
    {
        public CommandType CommandType => CommandType.SetFaceCulling;
        private bool _enable;
        private Face _face;

        public void Set(bool enable, Face face)
        {
            _enable = enable;
            _face = face;
        }

        public void Run(ThreadedRenderer threaded, IRenderer renderer)
        {
            renderer.Pipeline.SetFaceCulling(_enable, _face);
        }
    }
}
