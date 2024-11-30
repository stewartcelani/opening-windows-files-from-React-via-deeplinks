# Deeplinking Example

This is a minimal example of how to use deeplinking from a front-end website (React in this case).

In this specific example I was trying to find an easy and maintainable way to open files and folders on the local network in order to finalize migration of a legacy internal line-of-business Access Database 'app'.

I didn't want to have to maintain an application that needed an installer or required any dependencies. The solution is a single .EXE that just has to be run ONCE as an admin in order to register the protocol handler and from then on doesn't require any admin rights. This could easily be achieved using any RMM tool.

The protocol handler application is currently set up to listen for 'openfile' or 'openfolder' requests but this could be extended for other uses.

I left in the debug pauses and logs as this is just a proof of concept and I'm sharing as it may be of use to others.

![02. Example.gif](02.%20Example.gif)

## Usage on the front-end

```typescript jsx
 <div className="grid grid-cols-2 gap-4 max-w-2xl mx-auto">
    <FileLink
        path="C:\temp\test.xlsx"
        type="file"
        className="w-full px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
    >
        Open Excel File
    </FileLink>

    <FileLink
        path="C:\temp"
        type="folder"
        className="w-full px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition-colors"
    >
        Open Folder
    </FileLink>

    <FileLink
        path="C:\temp\test.xlsx"
        type="file"
        variant="link"
        className="text-blue-500 hover:text-blue-700 hover:underline"
    >
        Open Excel File
    </FileLink>

    <FileLink
        path="C:\temp"
        type="folder"
        variant="link"
        className="text-blue-500 hover:text-blue-700 hover:underline"
    >
        Open Folder
    </FileLink>
</div>
```

```typescript jsx
import {MouseEvent, ReactNode} from "react";

interface FileLinkProps {
    path: string;
    type: 'file' | 'folder';
    variant?: 'button' | 'link';
    children: ReactNode;
    className?: string;
}

export const FileLink = ({
                             path,
                             type,
                             variant = 'button',
                             children,
                             className = ''
                         }: FileLinkProps) => {
    const handleClick = (e: MouseEvent<HTMLElement>) => {
        e.preventDefault();

        // Ensure the path is properly formatted
        const normalizedPath = path.replace(/\\/g, '/');
        const encodedPath = encodeURIComponent(normalizedPath);
        const action = type === 'file' ? 'openfile' : 'openfolder';

        // Format: mycustomprotocolhandler://openfile/path
        const deepLink = `mycustomprotocolhandler://${action}/${encodedPath}`;

        console.log('Triggering deep link:', deepLink);

        try {
            window.location.href = deepLink;
        } catch (error) {
            console.error('Error triggering deep link:', error);
            alert('Failed to open item. Please ensure the protocol handler is installed.');
        }
    }

    const defaultButtonStyles = 'px-4 py-2 rounded bg-blue-500 hover:bg-blue-600 text-white transition-colors';
    const defaultLinkStyles = 'text-blue-600 hover:text-blue-800 hover:underline';

    // Choose base component and styles based on variant
    if (variant === 'link') {
        return (
            <a
                href="#"
                onClick={handleClick}
                className={`${defaultLinkStyles} ${className}`}
            >
                {children}
            </a>
        );
    }

    return (
        <button
            onClick={handleClick}
            className={`${defaultButtonStyles} ${className}`}
            type="button"
        >
            {children}
        </button>
    );
}
```