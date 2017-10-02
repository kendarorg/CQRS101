package org.cqrs101;

import com.fasterxml.jackson.databind.ObjectMapper;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.nio.ByteBuffer;
import java.nio.channels.FileChannel;
import java.nio.channels.FileLock;
import java.nio.channels.OverlappingFileLockException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.UUID;
import java.util.function.BiConsumer;
import java.util.function.Function;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.commons.lang3.StringUtils;

public class FileRepositoryHelper implements RepositoryHelper {

    private static String dbName = "tempdb";
    private static final ObjectMapper mapper = new ObjectMapper();
    private String name;
    private Class clazz;
    private String absolutePath;

    private FileChannel fc;
    private RandomAccessFile randomAccessFile;

    private Object actOnFile(Function<Object, Object> func) {

        try {
            RandomAccessFile randomAccessFile = new RandomAccessFile(absolutePath, "rw");
            fc = randomAccessFile.getChannel();
            ByteBuffer buffer = null;
            try (fc; randomAccessFile; FileLock fileLock = fc.tryLock()) {
                if (null != fileLock) {
                    buffer = ByteBuffer.wrap(data.getBytes());
                    buffer.put(data.toString().getBytes());
                    buffer.flip();
                    while (buffer.hasRemaining()) {
                        fc.write(buffer);
                    }
                }
            } catch (OverlappingFileLockException | IOException ex) {
                throw ex;
            }

            Object result = func.apply(fc);

            return result;
        } catch (Exception ex) {
            throw new RuntimeException("File handling error");
        }
    }

    @Override
    public RepositoryHelper create(Class clazz) {
        String path = System.getProperty("user.home");

        this.clazz = clazz;
        try {
            FileRepositoryHelper helper = new FileRepositoryHelper();
            helper.name = clazz.getSimpleName().toUpperCase(Locale.ROOT);
            helper.absolutePath = StringUtils.stripEnd(path, "\\/")
                    + File.pathSeparator + dbName + ".db";

            new File(helper.absolutePath).mkdirs();
            return helper;
        } catch (Exception ex) {
            throw new RuntimeException("Problem with file db", ex);
        }
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public Object getById(UUID id) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public List<Object> getAll() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public Object save(Object item, BiConsumer<Object, UUID> idSetter, Function<Object, UUID> idGetter) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    @Override
    public void delete(UUID id) {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
}
