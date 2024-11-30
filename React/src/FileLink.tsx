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