import './App.css'
import {FileLink} from "./FileLink.tsx";

function App() {
    return (
        <>
            <h1>Deeplinking Example</h1>
            <div className="p-8">
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
            </div>
        </>
    )
}

export default App